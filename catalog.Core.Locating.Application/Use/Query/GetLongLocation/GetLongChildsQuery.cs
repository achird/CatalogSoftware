using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о подчиненных местоположениях
/// </summary>
public class GetLongChildsQuery : IQuery<IList<LongLocationData>>
{
    /// <summary>
    /// Идентификатор местоположения
    /// </summary>
    public long ParentId { get; set; }
}
