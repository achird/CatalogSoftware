using catalog.Core.SharedKernel.Base.Specification;

namespace catalog.Core.Locating.Domain.Location;

/// <summary>
/// Фабрика спецификаций для LocationItem
/// </summary>
public class LocationObjectSpecificationFactory
{
    public LocationObjectSpecificationFactory()
    { }

    /// <summary>
    /// Условие по идентификатору
    /// </summary>
    /// <param name="id">идентификатор местности</param>
    /// <returns></returns>
    public Specification<LocationObject> ById(LocationObjectId id) => new AdHocSpecification<LocationObject>(a => a.Id == id);

    /// <summary>
    /// Условие по нескольким идентификаторам ГАР
    /// </summary>
    /// <param name="ids">несколько идентификаторов местности</param>
    /// <returns></returns>
    public Specification<LocationObject> ByIds(List<LocationObjectId> ids) => new AdHocSpecification<LocationObject>(a => ids.Contains(a.Id));

    /// <summary>
    /// Условие по нескольким идентификаторам ФИАС
    /// </summary>
    /// <param name="ids">несколько идентификаторов местности</param>
    /// <returns></returns>
    public Specification<LocationObject> ByIds(List<Guid> ids) => new AdHocSpecification<LocationObject>(a => ids.Contains(a.Uid));

    /// <summary>
    /// Условие по коду родительского элемента
    /// </summary>
    /// <param name="parentId">код родительского элемента</param>
    /// <returns></returns>
    public Specification<LocationObject> ByParentId(LocationObjectId parentId) => new AdHocSpecification<LocationObject>(a => a.ParentId == parentId);
}
