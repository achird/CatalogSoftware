namespace catalog.Infrastructure.Utility.XmlConverter.Common;

internal enum XmlElementType { Undefined, String, Number, DateTime, Property, Object, Array };

/// <summary>
/// Описание элемента xml-файла
/// </summary>
internal class XmlElement
{
    /// <summary>
    /// Создать описание элемента xml, без трансляции в свойство (без имени)
    /// </summary>
    /// <param name="name">имя элемента в xml</param>
    /// <param name="type">тип элемента</param>
    private XmlElement(string name, XmlElementType type)
    {
        Name = name;
        Type = type;
    }
    /// <summary>
    /// Создать описание именованного элемента xml, с трансляцией в свойство объекта
    /// </summary>
    /// <param name="name">имя элемента в xml</param>
    /// <param name="value">имя свойства в объекте</param>
    /// <param name="type">тип элемента</param>
    public XmlElement(string name, string value, XmlElementType type)
    {
        Name = name;
        Value = value;
        Type = type;
    }

    public string Name { get; }
    public string Value { get; }
    public XmlElementType Type { get; }

    public static readonly XmlElement Undefined = new(string.Empty, XmlElementType.Undefined);
    public static XmlElement Object(string name) => new(name, XmlElementType.Object);
    public static XmlElement Object(string name, string value) => new(name, value, XmlElementType.Object);
    public static XmlElement Array(string name) => new(name, XmlElementType.Array);
    public static XmlElement Array(string name, string value) => new(name, value, XmlElementType.Array);
    public static XmlElement Property(string name, string value) => new(name, value, XmlElementType.Property);
    public static XmlElement String(string name) => new(name, XmlElementType.String);
    public static XmlElement Number(string name) => new(name, XmlElementType.Number);
    public static XmlElement DateTime(string name) => new(name, XmlElementType.DateTime);

}
