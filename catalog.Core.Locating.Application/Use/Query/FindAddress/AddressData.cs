namespace catalog.Core.Locating.Application.Use.Query.FindAddress;

/// <summary>
/// Информация об адресе
/// </summary>
public class AddressData
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Идентификатор ФИАС
    /// </summary>
    public Guid Uid { get; set; }
    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string PostCode { get; set; }
    /// <summary>
    /// OKATO
    /// </summary>
    public string Okato { get; set; }
    /// <summary>
    /// Адрес населенного пункта
    /// </summary>
    public string Locality { get; set; }
    /// <summary>
    /// Улица
    /// </summary>
    public string Street { get; set; }
    /// <summary>
    /// Дом
    /// </summary>
    public string House { get; set; }
    /// <summary>
    /// Квартира
    /// </summary>
    public string Apartment { get; set; }
}
