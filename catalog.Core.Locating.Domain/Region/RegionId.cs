using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain.Region;

/// <summary>
/// Идентификатор региона
/// </summary>
public class RegionId : EntityId<RegionId>
{
    public RegionId(long value)
    {
        Value = value;
    }

    public static readonly RegionId Default = new RegionId(0);
}
