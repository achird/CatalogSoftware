namespace catalog.Core.Locating.Application.Use.Command.ImportCompleteRegion;

/// <summary>
/// Данные региона
/// </summary>
public class RegionData
{
    /// <summary>
    /// Идентификатор региона
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Код региона
    /// </summary>
    public string RegionCode { get; set; }
    /// <summary>
    /// Версия региона
    /// </summary>
    public DateTime Version { get; set; }
    /// <summary>
    /// Данные об адресных объектах
    /// </summary>
    public List<LocationObjectData> Objects { get; set; }
}
