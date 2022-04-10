using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о подчиненных местоположениях
/// </summary>
public class GetShortChildsQuery : IQuery<IList<ShortLocationData>>
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long ParentId { get; set; }
}
