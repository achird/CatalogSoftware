using catalog.Core.SharedKernel.Base.Domain;
using System.Text.RegularExpressions;

namespace catalog.Core.Locating.Domain.Region;

/// <summary>
/// Код региона
/// </summary>
public class RegionCode : ValueObject<RegionCode>
{
    private readonly string regionCodePattern = @"^\d{2}$";
    public RegionCode(string value)
    {
        if (value != string.Empty && !Regex.IsMatch(value, regionCodePattern))
            throw new ArgumentException(@"Некорректный формат значения кода региона");
        Value = value;
    }

    /// <summary>
    /// Код региона в виде строки
    /// </summary>
    public string Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Код региона по-умолчанию
    /// </summary>
    public static RegionCode Default => new(string.Empty);
}
