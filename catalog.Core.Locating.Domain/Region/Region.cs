using catalog.Core.Locating.Domain.Location;
using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Region;

/// <summary>
/// Представляет регион Российской Федерации
/// </summary>
public class Region : AggregateRoot<RegionId>
{
    private List<LocationObject> objects;
    private Region()
    {
    }
    public Region(RegionId id, RegionCode regionCode, RegionVersion version, IList<LocationObject> objects)
    {
        Id = id;
        RegionCode = regionCode ?? throw new ArgumentNullException(nameof(regionCode));
        Version = version ?? throw new ArgumentNullException(nameof(version));
        this.objects = objects.ToList() ?? throw new ArgumentNullException(nameof(objects));
    }

    /// <summary>
    /// Код региона
    /// </summary>
    public RegionCode RegionCode { get; }
    /// <summary>
    /// Версия региона
    /// </summary>
    public RegionVersion Version { get; }
    /// <summary>
    /// Список объектов местоположений региона
    /// </summary>
    public IReadOnlyList<LocationObject> Objects => objects;
}
