using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о подчиненных местоположениях
/// </summary>
public interface IGetShortChildsQueryHandler : IQueryHandler<GetShortChildsQuery, IList<ShortLocationData>>
{
}
