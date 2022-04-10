using catalog.Core.Application.Common.Cqrs;

namespace catalog.Core.Locating.Application.Use.Query.GetShortLocation;

/// <summary>
/// Получить КРАТКУЮ информацию о местоположениях
/// </summary>
public class GetShortLocationsQuery : IQuery<IReadOnlyDictionary<long, ShortLocationData>>
{
    /// <summary>
    /// Список идентификаторов для которых нужно загрузить информацию о местоположениях
    /// </summary>
    public IList<long> LocationIds { get; set; }
}
