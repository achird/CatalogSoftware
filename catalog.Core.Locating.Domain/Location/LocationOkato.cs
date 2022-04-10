using catalog.Core.SharedKernel.Base.Domain;
using System.Text.RegularExpressions;

namespace catalog.Core.Locating.Domain.Location;

/// <summary>
/// OKATO
/// </summary>
public class LocationOkato : ValueObject<LocationOkato>
{
    private LocationOkato(string value)
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
    /// <param name="value">OKATO</param>
    /// <returns></returns>
    public static LocationOkato Create(string value)
    {
        //if (value != string.Empty && !Regex.IsMatch(value, pattern))
        //throw new ArgumentException(@"Некорректный формат значения для OKATO");
        return string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern) ? Empty : new(value);
    }
    private static readonly string pattern = @"^\d{11}$";

    /// <summary>
    /// Пустое значение
    /// </summary>
    public static readonly LocationOkato Empty = new(string.Empty);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
