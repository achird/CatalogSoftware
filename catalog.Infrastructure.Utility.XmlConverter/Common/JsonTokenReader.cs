namespace catalog.Infrastructure.Utility.XmlConverter.Common;

/// <summary>
/// Класс конвертер из XmlToken в JsonToken согласно схеме
/// </summary>
internal class JsonTokenReader
{
    private readonly ISchema schema;
    private readonly XmlTokenReader tokenReader;
    private XmlElement value = XmlElement.Undefined;
    private JsonToken prevToken = JsonToken.Undefined, nextToken = JsonToken.Undefined;

    /// <summary>
    /// Класс конвертер из XmlToken в JsonToken согласно схеме
    /// </summary>
    /// <param name="schema">схема xml</param>
    /// <param name="tokenReader">источник данных</param>
    public JsonTokenReader(ISchema schema, XmlTokenReader tokenReader)
    {
        this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
        this.tokenReader = tokenReader ?? throw new ArgumentNullException(nameof(tokenReader));
    }

    private bool TryCreateValue(XmlElement value, XmlToken current, out JsonToken token)
    {
        if (value.Type != XmlElementType.Undefined &&
            prevToken.TokenType == JsonTokenType.PropertyName && current.TokenType == XmlTokenType.Value)
        {
            if (value.Type == XmlElementType.String)
            {
                token = JsonToken.String(current.Value);
                return true;
            }
            if (value.Type == XmlElementType.Number)
            {
                token = JsonToken.Number(current.Value);
                return true;
            }
            if (value.Type == XmlElementType.DateTime)
            {
                token = JsonToken.DateTime(current.Value);
                return true;
            }
        }

        token = JsonToken.Undefined;
        return false;
    }

    private bool TryCreateToken(XmlElement element, XmlToken current, out JsonToken currentToken)
    {
        if (element.Type == XmlElementType.Object)
        {
            if (current.TokenType == XmlTokenType.StartElement)
            {
                if (element.Value != null)
                {
                    currentToken = JsonToken.Property(element.Value);
                    nextToken = JsonToken.StartObject;
                    return true;
                }
                else
                {
                    currentToken = JsonToken.StartObject;
                    return true;
                }
            }
            if (current.TokenType == XmlTokenType.EndElement)
            {
                currentToken = JsonToken.EndObject;
                return true;
            }
        }

        if (element.Type == XmlElementType.Array)
        {
            if (current.TokenType == XmlTokenType.StartElement)
            {
                if (element.Value != null)
                {
                    currentToken = new JsonPropertyName(element.Value);
                    nextToken = JsonToken.StartArray;
                    return true;
                }
                else
                {
                    currentToken = JsonToken.StartArray;
                    return true;
                }
            }
            if (current.TokenType == XmlTokenType.EndElement)
            {
                currentToken = JsonToken.EndArray;
                return true;
            }
        }

        if (element.Type == XmlElementType.Property && (current.TokenType == XmlTokenType.AttributeName || current.TokenType == XmlTokenType.StartElement))
        {
            currentToken = JsonToken.Property(element.Value);
            return true;
        }

        if (prevToken.TokenType == JsonTokenType.PropertyName && current.TokenType == XmlTokenType.EndElement)
        {
            currentToken = JsonToken.Null;
            return true;
        }

        currentToken = JsonToken.Undefined;
        return false;
    }

    /// <summary>
    /// Попытаться прочитать следующий элемент
    /// </summary>
    /// <param name="token">результат чтения</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool Read(out JsonToken token)
    {
        // следующий элемент
        if (nextToken != JsonToken.Undefined)
        {
            token = nextToken;
            nextToken = JsonToken.Undefined;
            return true;
        }

        while (tokenReader.Read(out XmlToken current))
        {
            // создаем структурный элемент
            if (schema.TryGet(current.Value, out XmlElement element, out XmlElement value) &&
                TryCreateToken(element, current, out token))
            {
                this.value = value;
                prevToken = token;
                return true;
            }

            // cоздаем значение
            if (TryCreateValue(this.value, current, out token))
            {
                this.value = XmlElement.Undefined;
                prevToken = token;
                return true;
            }
        }

        // достигнут конец xml
        token = JsonToken.Undefined;
        return false;
    }

    private bool ReadObject(string objectName, IList<JsonToken> container)
    {
        var stack = new Stack<JsonToken>();

        // Поиск начала объекта
        while (Read(out JsonToken token))
        {
            if (token.TokenType == JsonTokenType.StartObject && tokenReader.CurrentPath == objectName)
            {
                // Чтение объекта
                do
                {
                    container.Add(token);

                    if (token.TokenType == JsonTokenType.StartObject)
                    {
                        stack.Push(token);
                    }

                    if (token.TokenType == JsonTokenType.EndObject && stack.TryPeek(out JsonToken temp) && temp.TokenType == JsonTokenType.StartObject)
                    {
                        stack.Pop();

                        // Если коллекция пуста, то достигнут конец объекта
                        if (stack.Count == 0)
                        {
                            return true;
                        }
                    }
                } while (Read(out token));
            }
        }

        return false;
    }

    /// <summary>
    /// Попытаться прочитать объект
    /// </summary>
    /// <param name="objectName">имя объекта в схеме</param>
    /// <param name="token">результат чтения</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadObject(string objectName, out JsonToken token)
    {
        var collection = new List<JsonToken>();
        if (ReadObject(objectName, collection))
        {
            token = JsonToken.Container(collection);
            return true;
        }

        token = JsonToken.Undefined;
        return false;
    }

    /// <summary>
    /// Попытаться прочитать колекцию объектов
    /// </summary>
    /// <param name="objectName">имя объекта в схеме</param>
    /// <param name="countObjectToRead">максимальное количество объектов для чтения</param>
    /// <param name="token">результат чтения</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadArray(string objectName, int countObjectToRead, out JsonToken token)
    {
        int numberOfObjects = 0;
        var collection = new List<JsonToken> { JsonToken.StartArray };
        while (ReadObject(objectName, collection))
        {
            // Достигнуто максимальное количество объектов?
            if (countObjectToRead - ++numberOfObjects == 0)
            {
                break;
            }
        }

        if (numberOfObjects != 0)
        {
            collection.Add(JsonToken.EndArray);
            token = JsonToken.Container(collection);
            return true;
        }

        token = JsonToken.Undefined;
        return false;
    }

    /// <summary>
    /// Попытаться прочитать колекцию объектов
    /// Выполняется чтение до конца потока
    /// </summary>
    /// <param name="objectName">имя объекта в схеме</param>
    /// <param name="token">результат чтения</param>
    /// <returns>успешно - true, в противном случае - false</returns>
    public bool ReadArray(string objectName, out JsonToken token)
    {
        return ReadArray(objectName, 0, out token);
    }
}
