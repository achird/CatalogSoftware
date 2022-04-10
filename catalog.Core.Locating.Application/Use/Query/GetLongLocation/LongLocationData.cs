namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Полная информация о местоположении
/// </summary>
public class LongLocationData
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Идентификатор родителя
    /// </summary>
    public long ParentId { get; set; }
    /// <summary>
    /// Уникальный идентификатор объекта из справочника ФИАС
    /// </summary>
    public Guid Uid { get; set; }
    /// <summary>
    /// Код уровня объекта
    /// </summary>
    public int LocationTypeCode { get; set; }
    /// <summary>
    /// Название уровня объекта
    /// </summary>
    public string LocationTypeName { get; set; }
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
    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Польное наименование объекта
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Имя собственное
    /// </summary>
    public string ProperName { get; set; }

    /// <summary>
    /// Уровень "Субъект РФ"
    /// </summary>
    public LocationObjectData Region { get; set; }
    /// <summary>
    /// Уровень "Административный район"
    /// </summary>
    public LocationObjectData Area { get; set; }
    /// <summary>
    /// Уровень "Город"
    /// </summary>
    public LocationObjectData City { get; set; }
    /// <summary>
    /// Уровень "Населенный пункт"
    /// </summary>
    public LocationObjectData Place { get; set; }
    /// <summary>
    /// Уровень "Элемент планировочной структуры"
    /// </summary>
    public LocationObjectData Struct { get; set; }
    /// <summary>
    /// Уровень "Элемент улично-дорожной сети"
    /// </summary>
    public LocationObjectData Street { get; set; }
    /// <summary>
    /// Уровень "Здание (сооружение)"
    /// </summary>
    public LocationObjectData House { get; set; }
    /// <summary>
    /// Уровень "Квартира (помещение)"
    /// </summary>
    public LocationObjectData Apartment { get; set; }
}
