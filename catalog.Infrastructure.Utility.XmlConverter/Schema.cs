using catalog.Infrastructure.Utility.XmlConverter.Common;

namespace catalog.Infrastructure.Utility.XmlConverter;

/// <summary>
/// Построитель схемы загрузки Xml
/// </summary>
public class Schema : ISchema
{
    private readonly Crumb crumb = new();
    private readonly List<XmlElement> schema = new();
    private ILookup<string, XmlElement> lookup = default;
    public Schema()
    {
    }

    /// <summary>
    /// Создать объект
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    public ISchema Object(string name, string value)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Object(crumb.CurrentPath, value));
        return this;
    }

    /// <summary>
    /// Создать объект
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <returns></returns>
    public ISchema Object(string name)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Object(crumb.CurrentPath));
        return this;
    }

    /// <summary>
    /// Создать массив
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    public ISchema Array(string name, string value)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Array(crumb.CurrentPath, value));
        return this;
    }

    /// <summary>
    /// Создать массив
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <returns></returns>
    public ISchema Array(string name)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Array(crumb.CurrentPath));
        return this;
    }

    /// <summary>
    /// Создать свойство (строка)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    public ISchema PropertyString(string name, string value)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Property(crumb.CurrentPath, value));
        schema.Add(XmlElement.String(crumb.CurrentPath));
        End();
        return this;
    }

    /// <summary>
    /// Создать свойство (число)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    public ISchema PropertyNumber(string name, string value)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Property(crumb.CurrentPath, value));
        schema.Add(XmlElement.Number(crumb.CurrentPath));
        End();
        return this;
    }

    /// <summary>
    /// Создать свойство (Дата и/или время)
    /// </summary>
    /// <param name="name">Относительный путь xml</param>
    /// <param name="value">Имя свойства</param>
    /// <returns></returns>
    public ISchema PropertyDateTime(string name, string value)
    {
        crumb.Push(name);
        schema.Add(XmlElement.Property(crumb.CurrentPath, value));
        schema.Add(XmlElement.DateTime(crumb.CurrentPath));
        End();
        return this;
    }

    /// <summary>
    /// Добавить уровень
    /// </summary>
    /// <returns></returns>
    public ISchema Add(string name)
    {
        crumb.Push(name);
        return this;
    }

    /// <summary>
    /// Понизить уровень
    /// </summary>
    /// <returns></returns>
    public ISchema End()
    {
        crumb.TryPop(out _);
        return this;
    }

    /// <summary>
    /// Получить элемент из схемы
    /// </summary>
    /// <param name="path">Путь к элементу</param>
    /// <param name="element">Элемент</param>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    bool ISchema.TryGet(string path, out XmlElement element, out XmlElement value)
    {
        if (lookup == default) lookup = schema.ToLookup(e => e.Name);
        if (lookup.Contains(path))
        {
            element = lookup[path].First();
            value = lookup[path].Skip(1).FirstOrDefault() ?? XmlElement.Undefined;
            return true;
        }

        element = XmlElement.Undefined;
        value = XmlElement.Undefined;
        return false;
    }
}
