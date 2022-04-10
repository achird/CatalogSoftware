using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

namespace catalog.Core.Exchange.Locating.Input;

/// <summary>
/// Получить данные регионов
/// </summary>
/// <returns>Перечисляемая коллекция регионов</returns>
public interface ILocatingGetter
{
    /// <summary>
    /// Получить список доступных регионов
    /// </summary>
    /// <param name="xmlPath">Путь к данным</param>
    /// <returns></returns>
    IList<XmlRegion> GetAllAvailableLocating(string xmlPath);

    /// <summary>
    /// Загрузить данные региона
    /// </summary>
    /// <param name="xmlPath">Путь к данным</param>
    /// <param name="region">Регион для загрузки данных</param>
    /// <returns></returns>
    XmlRegion GetLocatingData(string xmlPath, XmlRegion region);
}
