using catalog.Core.Application.Common.Service;
using catalog.Core.Locating.Application.Use;
using catalog.Core.Locating.Domain.Location;

namespace catalog.Core.Locating.Application.Interactor;

/// <summary>
/// Служба получения объектов местоположений
/// </summary>
public class CacheLocationGetter : ILocationGetter
{
    private readonly ILocationGetter locationGetter;
    private readonly ICaching<object, object> cachingObjects;
    private readonly ICaching<object, object> cachingParents;

    public CacheLocationGetter(ILocationGetter locationGetter, ICaching<object, object> cachingObjects, ICaching<object, object> cachingParents)
    {
        this.locationGetter = locationGetter ?? throw new ArgumentNullException(nameof(locationGetter));
        this.cachingObjects = cachingObjects ?? throw new ArgumentNullException(nameof(cachingObjects));
        this.cachingParents = cachingParents ?? throw new ArgumentNullException(nameof(cachingParents));
    }

    /// <summary>
    /// Найти местоположения по идентификаторам
    /// </summary>
    /// <param name="objects">Идентификаторы местоположений</param>
    private async Task<IEnumerable<object>> FillCacheAsync(Type type, IEnumerable<object> objects)
    {
        var distinct = objects.Distinct().ToList();
        var notCaching = distinct.Where(id => !cachingObjects.TryGet(id, out _)).ToList();
        if (notCaching.Any())
        {
            IReadOnlyDictionary<object, object>? toAdd = default;
            if (type == typeof(Guid))
                toAdd = (await locationGetter.GetAsync(notCaching.Cast<Guid>())).ToDictionary(l => (object)l.Key, l => (object)l.Value);
            if (type == typeof(LocationObjectId))
                toAdd = (await locationGetter.GetAsync(notCaching.Cast<LocationObjectId>())).ToDictionary(l => (object)l.Key, l => (object)l.Value);
            if (toAdd is { Count: > 0 }) 
                cachingObjects.AddRange(toAdd);
        }
        var notExist = distinct.Where(id => !cachingObjects.TryGet(id, out _)).ToList();
        if (notExist.Any())
            cachingObjects.AddRange(notExist.ToDictionary(l => l, l => (object)Location.Undefined));
        return distinct;
    }

    /// <summary>
    /// Найти местоположения по идентификаторам ГАР
    /// </summary>
    /// <param name="locationIds">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    public async Task<IReadOnlyDictionary<LocationObjectId, Location>> GetAsync(IEnumerable<LocationObjectId> locationIds)
    {
        return (await FillCacheAsync(typeof(LocationObjectId), locationIds))
            .ToDictionary(o => (LocationObjectId)o, o => (Location)cachingObjects.Get(o));
    }

    /// <summary>
    /// Найти местоположения по идентификаторам ФИАС
    /// </summary>
    /// <param name="locationUids">Идентификаторы местоположений</param>
    /// <returns>Объекты Location</returns>
    public async Task<IReadOnlyDictionary<Guid, Location>> GetAsync(IEnumerable<Guid> locationUids)
    {
        return (await FillCacheAsync(typeof(Guid), locationUids.Cast<object>()))
            .ToDictionary(o => (Guid)o, o => (Location)cachingObjects.Get(o));
    }

    /// <summary>
    /// Найти подчиненные элементы
    /// </summary>
    /// <param name="parentId">Идентификатор родителя</param>
    /// <returns>Объекты Location</returns>
    public async Task<IList<Location>> GetChildsAsync(LocationObjectId parentId)
    {
        if (cachingParents.TryGet(parentId, out var locations))
            return (IList<Location>)locations;

        locations = (await locationGetter.GetChildsAsync(parentId)).ToList();
        return (IList<Location>)cachingParents.GetOrAdd(parentId, locations);
    }
}
