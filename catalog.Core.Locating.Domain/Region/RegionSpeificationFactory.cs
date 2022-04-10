using catalog.Core.SharedKernel.Base.Specification;

namespace catalog.Core.Locating.Domain.Region;

/// <summary>
/// Фабрика спецификаций для Region
/// </summary>
public class RegionSpeificationFactory
{
    public RegionSpeificationFactory()
    { }

    /// <summary>
    /// Условие по коду региона и глубине загрузки
    /// </summary>
    /// <param name="regionCode">Код региона</param>
    /// <returns></returns>
    public Specification<Region> ByCode(string regionCode) => new AdHocSpecification<Region>(a => a.RegionCode.Value == regionCode);
}
