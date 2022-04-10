using System.Xml;

namespace catalog.Infrastructure.Utility.XmlConverter.Common;

/// <summary>
/// Класс чтения XmlToken из XmlReader
/// </summary>
internal class XmlTokenReader
{
    internal enum State { Undefined, StartElement, EndElement, ReadAttribute };
    internal enum AttributeState { ReadName, ReadValue };

    private readonly Crumb path = new();
    private readonly XmlReader xmlReader;
    private State currentState = State.Undefined;
    private AttributeState attributeState;

    public XmlTokenReader(XmlReader xmlReader)
    {
        this.xmlReader = xmlReader ?? throw new ArgumentNullException(nameof(xmlReader));
    }

    public string CurrentPath => path.CurrentPath;

    private bool ReadAttribute(out XmlToken token)
    {
        //AttributeName
        if (attributeState == AttributeState.ReadName && xmlReader.MoveToNextAttribute())
        {
            path.Push(xmlReader.Name);
            token = XmlToken.Attribute(path.CurrentPath);
            attributeState = AttributeState.ReadValue;
            return true;
        }

        //Value
        if (attributeState == AttributeState.ReadValue)
        {
            token = XmlToken.TokenValue(xmlReader.Value);
            attributeState = AttributeState.ReadName;
            path.TryPop(out _);
            return true;
        }

        xmlReader.MoveToElement();
        token = XmlToken.Undefined;
        return false;
    }

    private bool ReadElement(out XmlToken token)
    {
        //Если это пустой элемент, то создаем EndElement
        if (currentState != State.EndElement && xmlReader.IsEmptyElement)
        {
            currentState = State.EndElement;
            token = XmlToken.EndElement(path.CurrentPath);
            path.TryPop(out _);
            return true;
        }

        //Читаем следующий элемент
        while (xmlReader.Read())
        {
            //Value
            if (xmlReader.NodeType == XmlNodeType.Text)
            {
                token = XmlToken.TokenValue(xmlReader.Value);
                return true;
            }

            //StartElement
            if (xmlReader.NodeType == XmlNodeType.Element)
            {
                currentState = State.StartElement;
                path.Push(xmlReader.Name);
                token = XmlToken.StartElement(path.CurrentPath);

                if (xmlReader.HasAttributes)
                {
                    currentState = State.ReadAttribute;
                    attributeState = AttributeState.ReadName;
                }
                return true;
            }

            //EndElement
            if (xmlReader.NodeType == XmlNodeType.EndElement)
            {
                currentState = State.EndElement;
                token = XmlToken.EndElement(path.CurrentPath);
                path.TryPop(out _);
                return true;
            }
        }

        token = XmlToken.Undefined;
        return false;
    }

    /// <summary>
    /// Прочитать следующий элемент XmlToken
    /// </summary>
    /// <param name="token">результат чтения</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool Read(out XmlToken token)
    {
        if (currentState == State.ReadAttribute && ReadAttribute(out token))
        {
            return true;
        }
        if (ReadElement(out token))
        {
            return true;
        }
        return false;
    }
}
