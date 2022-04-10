using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о местоположениях
/// </summary>
public interface IGetLongLocationsQueryHandler : IQueryHandler<GetLongLocationsQuery, IReadOnlyDictionary<long, LongLocationData>>
{
}
