using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.FindAddress;

/// <summary>
/// Найти адреса по идентификаторам
/// </summary>
public interface IFindAddressesByIdQueryHandler : IQueryHandler<FindAddressesByIdQuery, IReadOnlyDictionary<long, AddressData>>
{
}
