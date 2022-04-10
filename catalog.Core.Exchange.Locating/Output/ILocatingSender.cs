using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

namespace catalog.Core.Exchange.Locating.Output;

/// <summary>
/// Отдаем данные в обработчик Acl
/// </summary>
public interface ILocatingSender
{
    /// <summary>
    /// Отправить данные регионов
    /// </summary>
    /// <param name="regions">Данные для обработки</param>
    /// <returns></returns>
    Task SendLocating(IList<XmlRegion> regions);
}
