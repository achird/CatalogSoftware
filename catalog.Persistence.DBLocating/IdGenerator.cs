using catalog.Core.Locating.Domain;
using catalog.Core.Locating.Domain.Region;
using catalog.Core.SharedKernel.Base.Domain;
using catalog.Persistence.DBLocating.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace catalog.Persistence.DBLocating;

/// <summary>
/// Генератор id для БД Financing через EF
/// Алгоритм HiLo
/// </summary>
public class IdGenerator : IIdGenerator
{
    /// <summary>
    /// Sequence в базе данных SqlServer
    /// </summary>
    class Sequence
    {
        private readonly object locker = new();
        private readonly string name;
        private long keyStart, keyValue, keyIncrement;

        public Sequence(string name)
        {
            this.name = name;
            keyStart = keyValue = keyIncrement = 0;
        }

        /// <summary>
        /// Получить параметры ключа
        /// </summary>
        public long GetKey(DbContextOptions<LocatingContext> options)
        {
            lock (locker)
            {
                if (keyValue >= keyStart + keyIncrement)
                {
                    using var context = new LocatingContext(options);

                    SqlParameter keyStartParam = new SqlParameter("@keyStart", System.Data.SqlDbType.BigInt) { Direction = System.Data.ParameterDirection.Output };
                    context.Database.ExecuteSqlRaw($"SELECT @keyStart = (NEXT VALUE FOR {name})", keyStartParam);
                    keyStart = keyValue = (long)keyStartParam.Value;

                    SqlParameter keyIncrementParam = new SqlParameter("@keyIncrement", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                    context.Database.ExecuteSqlRaw($"SELECT @keyIncrement = (SELECT cast(increment as int) FROM sys.sequences WHERE name = '{name}')", keyIncrementParam);
                    keyIncrement = (int)keyIncrementParam.Value;
                }

                return keyValue++;
            }
        }
    }

    private readonly Dictionary<Type, Sequence> sequences;
    private readonly DbContextOptions<LocatingContext> options;

    public IdGenerator(DbContextOptions<LocatingContext> options)
    {
        sequences = new Dictionary<Type, Sequence>();
        sequences.Add(typeof(RegionId), new Sequence("RegionHiLo"));
        this.options = options;
    }

    public T NewId<T>() where T : IEntityId
    {
        if (!sequences.TryGetValue(typeof(T), out var sequence))
            throw new ArgumentException($"Параметры ключа для {nameof(T)} отсутствуют");

        return (T)Activator.CreateInstance(typeof(T), sequence.GetKey(options));
    }
}
