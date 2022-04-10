using catalog.Core.Application.Common.Service;
using catalog.Core.Locating.Application.Persistence.QueryObject;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor;

/// <summary>
/// Служба получения объектов местоположений
/// </summary>
public class LocationGetter : ILocationGetter
{
    private readonly LocationObjectSpecificationFactory locationObjectSpecificationFactory;
    private readonly ILocationObjectQueryObject locationObjectQueryObject;
    private readonly ICaching<object, object> caching;

    public LocationGetter(LocationObjectSpecificationFactory locationObjectSpecificationFactory, ILocationObjectQueryObject locationObjectQueryObject, ICaching<object, object> caching)
    {
        this.locationObjectSpecificationFactory = locationObjectSpecificationFactory ?? throw new ArgumentNullException(nameof(locationObjectSpecificationFactory));
        this.locationObjectQueryObject = locationObjectQueryObject ?? throw new ArgumentNullException(nameof(locationObjectQueryObject));
        this.caching = caching ?? throw new ArgumentNullException(nameof(caching));
    }

    /// <summary>
    /// Заполнить кеш объектами LocationObjects
    /// </summary>
    /// <param name="locationObjects"></param>
    private async Task<IEnumerable<LocationObject>> FillCacheAsync(IEnumerable<LocationObject> locationObjects)
    {
        caching.AddRange(locationObjects.ToDictionary(l => (object)l.Id, l => (object)l));
        var structure = locationObjects
            .SelectMany(o => o.Structure).Distinct()
            .Where(id => !caching.TryGet(id, out _)).Select(id => id).ToList();
        if (structure.Any())
        {
            var structureObjects = await locationObjectQueryObject.GetAsync(locationObjectSpecificationFactory.ByIds(structure));
            caching.AddRange(structureObjects.ToDictionary(l => (object)l.Id, l => (object)l));
        }
        return locationObjects;
    }

    /// <summary>
    /// Создать объект Location для объекта местоположения
    /// </summary>
    /// <param name="locationObject"></param>
    /// <returns></returns>
    private Location GetLocation(LocationObject locationObject)
    {
        return new Location(locationObject.Structure.Select(id => (LocationObject)(caching.Get(id) ?? LocationObject.Default)).ToList());
    }

    /// <summary>
    /// Найти местоположения по идентификаторам ГАР
    /// </summary>
    /// <param name="locationIds">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    public async Task<IReadOnlyDictionary<LocationObjectId, Location>> GetAsync(IEnumerable<LocationObjectId> locationIds)
    {
        return (await FillCacheAsync(await locationObjectQueryObject.GetAsync(locationObjectSpecificationFactory.ByIds(locationIds.ToList()))))
            .ToDictionary(o => o.Id, o => GetLocation(o));
    }

    /// <summary>
    /// Найти местоположения по идентификаторам ФИАС
    /// </summary>
    /// <param name="locationIds">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    public async Task<IReadOnlyDictionary<Guid, Location>> GetAsync(IEnumerable<Guid> locationIds)
    {
        return (await FillCacheAsync(await locationObjectQueryObject.GetAsync(locationObjectSpecificationFactory.ByIds(locationIds.ToList()))))
            .ToDictionary(o => o.Uid, o => GetLocation(o));
    }

    /// <summary>
    /// Найти подчиненные элементы
    /// </summary>
    /// <param name="parentId">Идентификатор родителя</param>
    /// <returns>Объекты Location</returns>
    public async Task<IList<Location>> GetChildsAsync(LocationObjectId parentId)
    {
        return (await FillCacheAsync(await locationObjectQueryObject.GetAsync(locationObjectSpecificationFactory.ByParentId(parentId))))
            .Select(o => GetLocation(o)).ToList();
    }
}
