using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace catalog.Infrastructure.Utility.XmlConverter.Common;

internal enum JsonTokenType { Undefined, StartObject, EndObject, StartArray, EndArray, PropertyName, Value, Container }

/// <summary>
/// Абстрактный класс элемента json
/// </summary>
internal abstract class JsonToken : IEquatable<JsonToken>
{
    protected JsonToken(JsonTokenType tokenType)
    {
        TokenType = tokenType;
    }

    public JsonTokenType TokenType { get; }

    public abstract void Construct(StringWriter builder);

    public static bool operator ==(JsonToken value1, JsonToken value2)
    {
        if (value1 is null)
            return value2 is null;

        return value1.Equals(value2);
    }

    public static bool operator !=(JsonToken value1, JsonToken value2)
    {
        return !(value1 == value2);
    }

    public override bool Equals(object other)
    {
        if (other == null || GetType() != other.GetType())
            return false;

        return Compare(this, (JsonToken)other);
    }

    public bool Equals([AllowNull] JsonToken other)
    {
        if (other == null || GetType() != other.GetType())
            return false;

        return Compare(this, other);
    }

    public override int GetHashCode()
    {
        return TokenType.GetHashCode();
    }

    protected virtual bool Compare(JsonToken value1, JsonToken value2)
    {
        return value1.TokenType == value2.TokenType;
    }

    public static readonly JsonToken Undefined = new JsonUndefined();
    public static readonly JsonToken StartObject = new JsonStartObject();
    public static readonly JsonToken EndObject = new JsonEndObject();
    public static readonly JsonToken StartArray = new JsonStartArray();
    public static readonly JsonToken EndArray = new JsonEndArray();
    public static readonly JsonToken Null = new JsonNull();
    public static JsonToken String(string value) => new JsonString(value);
    public static JsonToken Number(string value) => new JsonNumber(value);
    public static JsonToken DateTime(string value) => new JsonDateTime(value);
    public static JsonToken Property(string value) => new JsonPropertyName(value);
    public static JsonToken Container(IList<JsonToken> tokens) => new JsonContainer(tokens);
}

/// <summary>
/// Неизвестный элемент
/// </summary>
internal class JsonUndefined : JsonToken
{
    public JsonUndefined()
        : base(JsonTokenType.Undefined)
    {
    }

    public override void Construct(StringWriter builder)
    {
    }
}

/// <summary>
/// Начало объекта
/// </summary>
internal class JsonStartObject : JsonToken
{
    public JsonStartObject()
        : base(JsonTokenType.StartObject)
    {
    }

    public override void Construct(StringWriter builder)
    {
        builder.Write("{");
    }
}

/// <summary>
/// Конец объекта
/// </summary>
internal class JsonEndObject : JsonToken
{
    public JsonEndObject()
        : base(JsonTokenType.EndObject)
    {
    }

    public override void Construct(StringWriter builder)
    {
        builder.Write("}");
    }
}

/// <summary>
/// Начало коллекции
/// </summary>
internal class JsonStartArray : JsonToken
{
    public JsonStartArray()
        : base(JsonTokenType.StartArray)
    {
    }

    public override void Construct(StringWriter builder)
    {
        builder.Write("[");
    }
}

/// <summary>
/// Конец коллекции
/// </summary>
internal class JsonEndArray : JsonToken
{
    public JsonEndArray()
        : base(JsonTokenType.EndArray)
    {
    }

    public override void Construct(StringWriter builder)
    {
        builder.Write("]");
    }
}

/// <summary>
/// Именованное свойство
/// </summary>
internal class JsonPropertyName : JsonToken
{
    public JsonPropertyName(string value)
        : base(JsonTokenType.PropertyName)
    {
        Value = value;
    }

    public string Value { get; protected set; }

    public override void Construct(StringWriter builder)
    {
        builder.Write($"\"{Value}\":");
    }

    protected override bool Compare(JsonToken value1, JsonToken value2)
    {
        return base.Compare(value1, value2) && ((JsonPropertyName)value1).Value == ((JsonPropertyName)value2).Value;
    }
    public override int GetHashCode()
    {
        return (TokenType, Value).GetHashCode();
    }
}

/// <summary>
/// Абстрактный класс 'значение'
/// </summary>
internal abstract class JsonValue : JsonToken
{
    public JsonValue(string value)
        : base(JsonTokenType.Value)
    {
        Value = value.Replace("\t", "").Replace("\n", "");
    }

    public string Value { get; protected set; }

    public override void Construct(StringWriter builder)
    {
        builder.Write(Value);
    }

    protected override bool Compare(JsonToken value1, JsonToken value2)
    {
        return base.Compare(value1, value2) && ((JsonValue)value1).Value == ((JsonValue)value2).Value;
    }
    public override int GetHashCode()
    {
        return (TokenType, Value).GetHashCode();
    }
}

/// <summary>
/// Значение - null
/// </summary>
internal class JsonNull : JsonValue
{
    public JsonNull()
        : base("null")
    {
    }
}

/// <summary>
/// Значение - строка
/// </summary>
internal class JsonString : JsonValue
{
    public JsonString(string value)
        : base(value)
    {
        Value = $"\"{Value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }
}

/// <summary>
/// Значение - число
/// </summary>
internal class JsonNumber : JsonValue
{
    public JsonNumber(string value)
        : base(value)
    {
    }
}

/// <summary>
/// Значение - дата и время
/// </summary>
internal class JsonDateTime : JsonValue
{
    static readonly CultureInfo enUS = new CultureInfo("en-US");
    public JsonDateTime(string value)
        : base(value)
    {
        Value = TryGetDateTime(Value, out DateTime result)
            ? $"\"{result:s}\""
            : "null";
    }
    private bool TryGetDateTime(string value, out DateTime result)
    {
        if (System.DateTime.TryParseExact(value, "yyyyMMdd", enUS, DateTimeStyles.None, out result))
            return true;
        if (System.DateTime.TryParse(value, out result))
            return true;
        return false;
    }
}

/// <summary>
/// Коллекция элементов  JToken
/// </summary>
internal class JsonContainer : JsonToken, IEnumerable<JsonToken>
{
    private readonly ICollection<JsonToken> collection;

    public JsonContainer(IList<JsonToken> tokens)
        : base(JsonTokenType.Container)
    {
        collection = tokens;
    }

    public override void Construct(StringWriter builder)
    {
        var prev = Undefined;
        foreach (var next in collection)
        {
            if ((prev.TokenType == JsonTokenType.Value ||
                 prev.TokenType == JsonTokenType.EndObject ||
                 prev.TokenType == JsonTokenType.EndArray) &&
                (next.TokenType == JsonTokenType.PropertyName ||
                 next.TokenType == JsonTokenType.Value ||
                 next.TokenType == JsonTokenType.StartObject ||
                 next.TokenType == JsonTokenType.StartArray))
            {
                builder.Write(",");
            }

            next.Construct(builder);
            prev = next;
        }
    }

    protected override bool Compare(JsonToken value1, JsonToken value2)
    {
        if (base.Compare(value1, value2))
        {
            var container1 = (JsonContainer)value1;
            var container2 = (JsonContainer)value2;

            return container1.SequenceEqual(container2);
        }

        return false;
    }

    public IEnumerator<JsonToken> GetEnumerator()
    {
        return collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
