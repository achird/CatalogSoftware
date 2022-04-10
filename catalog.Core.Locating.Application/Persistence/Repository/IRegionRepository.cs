using catalog.Core.Locating.Domain.Region;
using catalog.Core.SharedKernel.Base.Specification;

namespace catalog.Core.Locating.Application.Persistence.Repository;

/// <summary>
/// Репозиторий региона
/// </summary>
public interface IRegionRepository
{
    /// <summary>
    /// Получить список Region по условию
    /// </summary>
    /// <param name="specification">условие отбора</param>
    /// <returns>список объектов Region</returns>
    Task<IList<Region>> GetAsync(Specification<Region> specification);

    /// <summary>
    /// Удалить все регионы
    /// </summary>
    /// <returns></returns>
    Task RemoveAllAsync();

    /// <summary>
    /// Добавить регионы
    /// </summary>
    /// <param name="regions">Данные для обновления</param>
    /// <returns></returns>
    Task AddAsync(IList<Region> regions);
}
