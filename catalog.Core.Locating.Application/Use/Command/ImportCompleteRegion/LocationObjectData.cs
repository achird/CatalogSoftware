namespace catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

/// <summary>
/// Информация об объекте местоположения
/// </summary>
public class LocationObjectData
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Регион
    /// </summary>
    public long RegionId { get; set; }
    /// <summary>
    /// Уникальный идентификатор объекта из справочника ФИАС
    /// </summary>
    public Guid Uid { get; set; }
    /// <summary>
    /// Уровень объекта
    /// </summary>
    public int LocationType { get; set; }
    /// <summary>
    /// Идентификатор родительского объекта
    /// </summary>
    public long ParentId { get; set; }
    /// <summary>
    /// Содержит иерархию объектов местоположений от Субъекта РФ до текущего объекта 
    /// </summary>
    public List<long> Structure { get; set; }
    /// <summary>
    /// Имя собственное
    /// </summary>
    public string ProperName { get; set; }
    /// <summary>
    /// Полное наименование объекта
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Код адресного объекта одной строкой без признака актуальности
    /// </summary>
    public string PlainCode { get; set; }
    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string PostCode { get; set; }
    /// <summary>
    /// OKATO
    /// </summary>
    public string Okato { get; set; }
}
