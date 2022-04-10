using catalog.Core.Locating.Application.Persistence.Repository;
using catalog.Core.Locating.Domain.Location;
using catalog.Core.Locating.Domain.Region;
using catalog.Core.SharedKernel.Base.Specification;
using catalog.Persistence.DBLocating.Context;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace catalog.Persistence.DBLocating.Repository;

/// <summary>
/// Репозиторий регионов.
/// Реализация через EF Core, БД Locating
/// </summary>
public class RegionRepository : IRegionRepository
{
    private readonly DbContextOptions<LocatingContext> options;

    public RegionRepository(DbContextOptions<LocatingContext> options)
    {
        this.options = options;
    }

    /// <summary>
    /// Получить список Region по условию
    /// </summary>
    /// <param name="specification">условие отбора</param>
    /// <returns>список объектов Region</returns>
    public async Task<IList<Region>> GetAsync(Specification<Region> specification)
    {
        using var context = new LocatingContext(options);
        return await context.Regions.Where(specification.ToExpression()).ToListAsync();
    }

    /// <summary>
    /// Удалить все регионы
    /// </summary>
    /// <returns></returns>
    public async Task RemoveAllAsync()
    {
        using var context = new LocatingContext(options);
        // Удалить данные
        await context.TruncateAsync<LocationObject>();
        await context.Regions.BatchDeleteAsync();
    }

    /// <summary>
    /// Безопасно выполнить команду
    /// </summary>
    /// <param name="command">команда</param>
    /// <returns></returns>
    private void GuardSqlCommand(Action command, Action? afterException, string message)
    {
        // Игнорируем ошибку
        bool reply = true;
        while (reply)
        {
            try
            {
                reply = false;
                command();
            }
            catch (SqlException local)
            {
                if (local.Message.ToLower().Contains(message))
                {
                    afterException?.Invoke();
                    reply = true;
                }
                else
                {
                    // Неизвестная ошибка
                    throw local;
                }
            }
        }
    }

    /// <summary>
    /// Добавить регион
    /// </summary>
    /// <param name="regions">Данные для обновления</param>
    /// <returns></returns>
    public async Task AddAsync(IList<Region> regions)
    {
        using var context = new LocatingContext(options);
        context.Database.SetCommandTimeout(0);
        var locatingCountBefore = await context.Regions.CountAsync();
        // Урезать базу данных
        if (locatingCountBefore == 0)
        {
            var databaseName = context.Database.GetDbConnection().Database;
            await context.Database.ExecuteSqlRawAsync($"DBCC SHRINKFILE(N'{databaseName}', 1024); DBCC SHRINKFILE(N'{databaseName}_log', 256); ");
        }
        // Проверяем регионы
        if (await context.Regions.AnyAsync(l => regions.Select(x => x.RegionCode).Contains(l.RegionCode)))
            throw new InvalidOperationException("Один из регионов уже загружен в базу");
        // Добавляем регионы
        context.AddRange(regions);
        await context.SaveChangesAsync();
        // Вставляем данные
        foreach (var region in regions)
        {
            foreach (var partition in Split(region.Objects))
            {
                // Безопасная вставка данных
                GuardSqlCommand(
                    () => GuardSqlCommand(
                        () => context.BulkInsert(partition.ToList()),
                        () => context.BulkDelete(partition.ToList()),
                        "insert duplicate key"),
                    default,
                    "timeout");
            }
        }
        // Перестраиваем индекс и урезаем базу данных после загрузки 30 регионов
        var locatingCountAfter = await context.Regions.CountAsync();
        if (locatingCountBefore / 30 - locatingCountAfter / 30 != 0)
        {
            var databaseName = context.Database.GetDbConnection().Database;
            await context.Database.ExecuteSqlRawAsync($"ALTER INDEX ALL ON [LocationObject] REBUILD;");
            await context.Database.ExecuteSqlRawAsync($"DBCC SHRINKFILE(N'{databaseName}', 1024); DBCC SHRINKFILE(N'{databaseName}_log', 256); ");
            await context.Database.ExecuteSqlRawAsync($"ALTER INDEX ALL ON [LocationObject] REORGANIZE;");
        }
    }

    /// <summary>
    /// Разбить данные на блоки
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    /// <param name="entities">Перечисляемая коллекция элементов</param>
    /// <param name="defaultBlockSize">Необходимый размер блока</param>
    /// <returns></returns>
    private static IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> entities, int defaultBlockSize = 10000)
    {
        for (int count = 0; count < entities.Count(); count += defaultBlockSize)
            yield return entities.Skip(count).Take(defaultBlockSize);
    }
}
