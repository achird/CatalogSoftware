using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.FindAddress;

/// <summary>
/// Получить информацию об адресах
/// </summary>
public class FindAddressesByIdQuery : IQuery<IReadOnlyDictionary<long, AddressData>>
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public IList<long> LocationIds { get; set; }
}
