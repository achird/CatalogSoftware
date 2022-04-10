using catalog.Core.Locating.Domain.Location;
using catalog.Core.SharedKernel.Base.Specification;

namespace catalog.Core.Locating.Application.Persistence.QueryObject;

/// <summary>
/// QueryObject для получения данных об объектах местоположений
/// </summary>
public interface ILocationObjectQueryObject
{
    /// <summary>
    /// Загрузить данные об объектах местоположений в соответствии с условием
    /// </summary>
    /// <param name="specification">Условие для загрузки данных</param>
    /// <returns></returns>
    Task<List<LocationObject>> GetAsync(Specification<LocationObject> specification);
}
