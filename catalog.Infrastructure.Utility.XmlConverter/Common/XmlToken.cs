namespace catalog.Infrastructure.Utility.XmlConverter.Common;

internal enum XmlTokenType { Undefined, StartElement, EndElement, AttributeName, Value }

/// <summary>
/// Элемент xml-файла
/// </summary>
internal class XmlToken
{
    private XmlToken(XmlTokenType tokenType, string value)
    {
        TokenType = tokenType;
        Value = value;
    }

    public XmlTokenType TokenType { get; }
    public string Value { get; }

    public static readonly XmlToken Undefined = new(XmlTokenType.Undefined, string.Empty);
    public static XmlToken StartElement(string path) => new(XmlTokenType.StartElement, path);
    public static XmlToken EndElement(string path) => new(XmlTokenType.EndElement, path);
    public static XmlToken Attribute(string path) => new(XmlTokenType.AttributeName, path);
    public static XmlToken TokenValue(string path) => new(XmlTokenType.Value, path);

}
