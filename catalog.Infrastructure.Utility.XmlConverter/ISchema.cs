using catalog.Infrastructure.Utility.XmlConverter.Common;

namespace catalog.Infrastructure.Utility.XmlConverter;

public interface ISchema
{
    /// <summary>
    /// Создать массив
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <returns></returns>
    ISchema Array(string name);

    /// <summary>
    /// Создать массив
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    ISchema Array(string name, string value);

    /// <summary>
    /// Создать объект
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <returns></returns>
    ISchema Object(string name);
    /// <summary>
    /// Создать объект
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    ISchema Object(string name, string value);

    /// <summary>
    /// Создать свойство (Дата и/или время)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    ISchema PropertyDateTime(string name, string value);

    /// <summary>
    /// Создать свойство (число)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    ISchema PropertyNumber(string name, string value);

    /// <summary>
    /// Создать свойство (строка)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    ISchema PropertyString(string name, string value);

    /// <summary>
    /// Добавить уровень
    /// </summary>
    /// <returns></returns>
    ISchema Add(string name);

    /// <summary>
    /// Понизить уровень
    /// </summary>
    /// <returns></returns>
    ISchema End();

    /// <summary>
    /// Получить схему
    /// </summary>
    internal bool TryGet(string path, out XmlElement element, out XmlElement value);
}
