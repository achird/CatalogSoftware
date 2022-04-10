using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.FindAddress;

/// <summary>
/// Найти адреса по уникальным идентификаторам
/// </summary>
public interface IFindAddressesByUidQueryHandler : IQueryHandler<FindAddressesByUidQuery, IReadOnlyDictionary<Guid, AddressData>>
{
}
