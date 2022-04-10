using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Use;

/// <summary>
/// Служба получения объектов местоположений
/// </summary>
public interface ILocationGetter
{
    /// <summary>
    /// Найти местоположения по идентификаторам
    /// </summary>
    /// <param name="locationIds">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    Task<IReadOnlyDictionary<LocationObjectId, Location>> GetAsync(IEnumerable<LocationObjectId> locationIds);

    /// <summary>
    /// Найти местоположения по идентификаторам Uid
    /// </summary>
    /// <param name="locationIds">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    Task<IReadOnlyDictionary<Guid, Location>> GetAsync(IEnumerable<Guid> locationIds);

    /// <summary>
    /// Найти подчиненные элементы
    /// </summary>
    /// <param name="parentId">Идентификатор родителя</param>
    /// <returns>Объекты Location</returns>
    Task<IList<Location>> GetChildsAsync(LocationObjectId parentId);
}
