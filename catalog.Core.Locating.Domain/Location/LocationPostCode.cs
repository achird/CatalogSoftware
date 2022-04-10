using catalog.Core.SharedKernel.Base.Domain;
using System.Text.RegularExpressions;

namespace catalog.Core.Locating.Domain.Location;

/// <summary>
/// Почтовый индекс
/// </summary>
public class LocationPostCode : ValueObject<LocationPostCode>
{
    private LocationPostCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Значение
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Создать экземпляр класса
    /// </summary>
    /// <param name="value">Почтовый индекс</param>
    /// <returns></returns>
    public static LocationPostCode Create(string value)
    {
        //if (value != string.Empty && !Regex.IsMatch(value, pattern))
        //    throw new ArgumentException(@"Некорректный формат значения для PostCode");
        return string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern) ? Empty : new(value);
    }
    private static readonly string pattern = @"^\d{6}$";

    /// <summary>
    /// Пустое значение
    /// </summary>
    public static readonly LocationPostCode Empty = new(string.Empty);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
