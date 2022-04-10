using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о подчиненных местоположениях
/// </summary>
public interface IGetLongChildsQueryHandler : IQueryHandler<GetLongChildsQuery, IList<LongLocationData>>
{
}
