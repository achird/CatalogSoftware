namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Информация об адресном объекте
/// </summary>
public class LocationObjectData
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Уникальный идентификатор объекта из справочника ФИАС
    /// </summary>
    public Guid Uid { get; set; }
    /// <summary>
    /// Уровень объекта
    /// </summary>
    public int LocationTypeCode { get; set; }
    /// <summary>
    /// Наименование уровня объекта
    /// </summary>
    public string LocationTypeName { get; set; }
    /// <summary>
    /// Имя собственное
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Польное наименование объекта
    /// </summary>
    public string ProperName { get; set; }
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
