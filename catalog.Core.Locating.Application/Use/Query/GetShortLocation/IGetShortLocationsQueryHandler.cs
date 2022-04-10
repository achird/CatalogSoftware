using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о местоположениях
/// </summary>
public interface IGetShortLocationsQueryHandler : IQueryHandler<GetShortLocationsQuery, IReadOnlyDictionary<long, ShortLocationData>>
{
}
