using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Region;

/// <summary>
/// Версия региона
/// </summary>
public class RegionVersion : ValueObject<RegionVersion>, IComparable<RegionVersion>
{
    public RegionVersion(DateTime updateDate)
    {
        UpdateDate = updateDate;
    }

    /// <summary>
    /// Дата обновления данных
    /// </summary>
    public DateTime UpdateDate { get; }

    public int CompareTo(RegionVersion other)
    {
        return UpdateDate.CompareTo(other.UpdateDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UpdateDate;
    }

    /// <summary>
    /// Версия региона по-умолчанию
    /// </summary>
    public static RegionVersion Default => new(new DateTime(2011, 1, 1));
}
