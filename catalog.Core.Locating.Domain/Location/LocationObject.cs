using catalog.Core.Locating.Domain.Region;
using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Location;

/// <summary>
/// Информация об объекте местоположения
/// </summary>
public class LocationObject : Entity<LocationObjectId>
{
    private readonly List<LocationObjectId> structure;
    private LocationObject()
    {
    }
    private LocationObject(LocationObjectId id, RegionId regionId, Guid uid, LocationType locationType, LocationObjectId parentId, IList<LocationObjectId> structure, string properName, string name, LocationPlainCode plainCode, LocationOkato okato, LocationPostCode postCode)
    {
        Id = id;
        Uid = uid;
        ParentId = parentId;
        RegionId = regionId ?? throw new ArgumentNullException(nameof(regionId));
        LocationType = locationType ?? LocationType.Undefined;
        ProperName = properName;
        Name = name;
        PlainCode = plainCode;
        Okato = okato;
        PostCode = postCode;

        this.structure = (structure ?? throw new ArgumentNullException(nameof(structure))).ToList();
    }
    /// <summary>
    /// Уникальный идентификатор объекта из справочника ФИАС
    /// </summary>
    public Guid Uid { get; }
    /// <summary>
    /// Уровень объекта
    /// </summary>
    public LocationType LocationType { get; }
    /// <summary>
    /// Регион
    /// </summary>
    public RegionId RegionId { get; }
    /// <summary>
    /// Идентификатор родительского объекта
    /// </summary>
    public LocationObjectId ParentId { get; }
    /// <summary>
    /// Имя собственное
    /// </summary>
    public string ProperName { get; }
    /// <summary>
    /// Полное наименование объекта
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Код адресного объекта одной строкой без признака актуальности
    /// </summary>
    public LocationPlainCode PlainCode { get; set; }
    /// <summary>
    /// OKATO
    /// </summary>
    public LocationOkato Okato { get; }
    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public LocationPostCode PostCode { get; }

    /// <summary>
    /// Содержит иерархию объектов местоположений от Субъекта РФ до текущего объекта 
    /// </summary>
    public IReadOnlyList<LocationObjectId> Structure => structure;

    /// <summary>
    /// Пустой объект местоположения
    /// </summary>
    /// <returns></returns>
    public bool IsDefault() => this == Default;

    /// <summary>
    /// Представление объекта местоположения в виде строки
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;

    /// <summary>
    /// Объект местоположения по-умолчанию
    /// </summary>
    public readonly static LocationObject Default = new(
        LocationObjectId.Default,
        RegionId.Default,
        Guid.Empty,
        LocationType.Undefined,
        LocationObjectId.Default,
        new List<LocationObjectId>(),
        string.Empty,
        string.Empty,
        LocationPlainCode.Empty,
        LocationOkato.Empty,
        LocationPostCode.Empty
    );
}
