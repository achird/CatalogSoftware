using System.Xml;
using catalog.Infrastructure.Utility.XmlConverter.Common;

namespace catalog.Infrastructure.Utility.XmlConverter;

/// <summary>
/// Создать класс чтения объектов из xml-файлов
/// </summary>
public class XmlObjectReader : IDisposable
{
    private readonly XmlReader xmlReader;
    private readonly JsonTokenReader tokenReader;

    /// <summary>
    /// Создать класс чтения объектов из xml-файлов
    /// </summary>
    /// <param name="xmlSchema">схема xml</param>
    /// <param name="xmlUri">путь к xml файлу</param>
    public XmlObjectReader(ISchema xmlSchema, string xmlUri)
    {
        xmlReader = XmlReader.Create(xmlUri);
        tokenReader = new JsonTokenReader(xmlSchema, new XmlTokenReader(xmlReader));
    }

    /// <summary>
    /// Создать класс чтения объектов из xml-файлов
    /// </summary>
    /// <param name="xmlSchema">схема xml</param>
    /// <param name="xmlStream">исходные данные xml</param>
    public XmlObjectReader(ISchema xmlSchema, Stream xmlStream)
    {
        xmlReader = XmlReader.Create(xmlStream ?? throw new ArgumentNullException(nameof(xmlStream)));
        tokenReader = new JsonTokenReader(xmlSchema ?? throw new ArgumentNullException(nameof(xmlSchema)), new XmlTokenReader(xmlReader));
    }

    /// <summary>
    /// Попытаться прочитать объект типа T
    /// </summary>
    /// <typeparam name="T">тип объекта</typeparam>
    /// <param name="objectName">имя объекта в схеме</param>
    /// <param name="value">экземпляр объекта</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadObject<T>(string objectName, out T value)
    {
        if (tokenReader.ReadObject(objectName, out JsonToken token))
        {
            using var stringWriter = new StringWriter();
            token.Construct(stringWriter);
            value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringWriter.ToString());
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Попытаться прочитать коллекцию объектов типа T
    /// </summary>
    /// <typeparam name="T">Тип объектов в коллекции</typeparam>
    /// <param name="objectName">Название объекта в схеме</param>
    /// <param name="maxObjectCount">максимальное количество объектов для чтения</param>
    /// <param name="collection">коллекция объектов</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadCollection<T>(string objectName, int maxObjectCount, out IEnumerable<T> collection)
    {
        if (tokenReader.ReadArray(objectName, maxObjectCount, out JsonToken token))
        {
            using var stringWriter = new StringWriter();
            token.Construct(stringWriter);
            collection = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T>>(stringWriter.ToString());
            return true;
        }

        collection = default;
        return false;
    }

    /// <summary>
    /// Попытаться прочитать коллекцию объектов типа T
    /// Выполняется чтение до конца потока
    /// </summary>
    /// <typeparam name="T">Тип объектов в коллекции</typeparam>
    /// <param name="objectName">имя объекта в схеме</param>
    /// <param name="collection">коллекция объектов</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadCollection<T>(string objectName, out IEnumerable<T> collection)
    {
        return ReadCollection(objectName, 0, out collection);
    }

    public void Dispose()
    {
        if (xmlReader != null)
        {
            xmlReader.Dispose();
        }
    }
}
