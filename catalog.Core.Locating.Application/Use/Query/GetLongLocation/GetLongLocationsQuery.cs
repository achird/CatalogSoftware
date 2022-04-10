using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetLongLocation;

/// <summary>
/// Получить ПОЛНУЮ информацию о местоположениях
/// </summary>
public class GetLongLocationsQuery : IQuery<IReadOnlyDictionary<long, LongLocationData>>
{
    /// <summary>
    /// Список идентификаторов для которых нужно загрузить информацию о местоположениях
    /// </summary>
    public IList<long> LocationIds { get; set; }
}
