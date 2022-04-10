using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.FindAddress;

/// <summary>
/// Получить информацию об адресах
/// </summary>
public class FindAddressesByUidQuery : IQuery<IReadOnlyDictionary<Guid, AddressData>>
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public IList<Guid> LocationUids { get; set; }
}
