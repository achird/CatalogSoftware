using catalog.Infrastructure.Utility.XmlConverter.Common;
using catalog.Test.Infrastructure.Utility.XmlConverter.Setup;
using System.Collections.Generic;
using System.Xml;
using Xunit;

namespace catalog.Test.Infrastructure.Utility.XmlConverter;

public class JsonTokenReaderTests
{
    (List<JsonToken>, Stack<JsonToken>) ReadJsonToken(JsonTokenReader tokenReader)
    {
        var tokens = new List<JsonToken>();
        var stack = new Stack<JsonToken>();
        while (tokenReader.Read(out JsonToken token))
        {
            if (token.TokenType == JsonTokenType.StartArray || token.TokenType == JsonTokenType.StartObject)
            {
                stack.Push(token);
            }
            if (token.TokenType == JsonTokenType.EndArray && stack.TryPeek(out JsonToken temp) && temp.TokenType == JsonTokenType.StartArray)
            {
                stack.Pop();
            }
            if (token.TokenType == JsonTokenType.EndObject && stack.TryPeek(out temp) && temp.TokenType == JsonTokenType.StartObject)
            {
                stack.Pop();
            }
            tokens.Add(token);
        }
        return (tokens, stack);
    }

    [Fact]
    public void TestJsonToken()
    {
        using var flatXmlClass = XmlSetup.GenerateXmlClass();
        using var xmlReaderClass = XmlReader.Create(flatXmlClass);
        var tokenReader = new JsonTokenReader(XmlSetup.GetXmlClassSchema(), new XmlTokenReader(xmlReaderClass));

        var (tokens, stack) = ReadJsonToken(tokenReader);
        //Ожидается коллекция из 36 элемента
        Assert.Equal(36, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);

        using var flatXmlCollection = XmlSetup.GenerateXmlCollection();
        using var xmlReaderCollection = XmlReader.Create(flatXmlCollection);
        tokenReader = new JsonTokenReader(XmlSetup.GetXmlCollectionSchema(), new XmlTokenReader(xmlReaderCollection));

        (tokens, stack) = ReadJsonToken(tokenReader);
        //Ожидается коллекция из 82 элементов
        Assert.Equal(82, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);

        using var flatXmlComplexClass = XmlSetup.GenerateXmlComplexClass();
        using var xmlReaderComplexClass = XmlReader.Create(flatXmlComplexClass);
        tokenReader = new JsonTokenReader(XmlSetup.GetXmlComplexClassSchema(), new XmlTokenReader(xmlReaderComplexClass));

        (tokens, stack) = ReadJsonToken(tokenReader);
        //Ожидается коллекция из 54 элементов
        Assert.Equal(54, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);
    }

    [Fact]
    public void TestObjectContainer()
    {
        using var flatXmlClass = XmlSetup.GenerateXmlClass();
        using var xmlReaderClass = XmlReader.Create(flatXmlClass);
        var tokenReader = new JsonTokenReader(XmlSetup.GetXmlClassSchema(), new XmlTokenReader(xmlReaderClass));

        tokenReader.ReadObject("packet.zglv", out JsonToken actual);
        Assert.Equal(JsonToken.Container(new List<JsonToken>()
        {
            JsonToken.StartObject,

            JsonToken.Property("Version"),
            JsonToken.Number("1.1"),
            JsonToken.Property("UpdateDate"),
            JsonToken.DateTime("03.10.2019"),

            JsonToken.EndObject
        }), actual);

        tokenReader.ReadObject("packet.entry", out actual);
        Assert.Equal(JsonToken.Container(new List<JsonToken>()
        {
            JsonToken.StartObject,

            JsonToken.Property("Code"),
            JsonToken.Number("0"),
            JsonToken.Property("Name"),
            JsonToken.String("Не требуется изготовление временного свидетельства"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.10.2019"),
            JsonToken.Property("DateEnd"),
            JsonToken.Null,

            JsonToken.EndObject
        }), actual);

        tokenReader.ReadArray("packet.entry", out actual);
        Assert.Equal(JsonToken.Container(new List<JsonToken>()
        {
            JsonToken.StartArray,

            JsonToken.StartObject,
            JsonToken.Property("Code"),
            JsonToken.Number("1"),
            JsonToken.Property("Name"),
            JsonToken.String("Временное свидетельство на бумажном бланке"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.05.2011"),
            JsonToken.Property("DateEnd"),
            JsonToken.Null,
            JsonToken.EndObject,

            JsonToken.StartObject,
            JsonToken.Property("Code"),
            JsonToken.Number("2"),
            JsonToken.Property("Name"),
            JsonToken.String("Временное свидетельство в электронной форме"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.06.2019"),
            JsonToken.Property("DateEnd"),
            JsonToken.Null,
            JsonToken.EndObject,

            JsonToken.EndArray
        }), actual);

        using var flatXmlCollection = XmlSetup.GenerateXmlCollection();
        using var xmlReaderCollection = XmlReader.Create(flatXmlCollection);
        tokenReader = new JsonTokenReader(XmlSetup.GetXmlCollectionSchema(), new XmlTokenReader(xmlReaderCollection));

        tokenReader.ReadArray("root.rec", out actual);
        Assert.Equal(JsonToken.Container(new List<JsonToken>()
        {
            JsonToken.StartArray,

            JsonToken.StartObject,
            JsonToken.Property("CodeMo"),
            JsonToken.String("750004"),
            JsonToken.Property("Unit"),
            JsonToken.Number("1"),
            JsonToken.Property("District"),
            JsonToken.Number("1"),
            JsonToken.Property("Name"),
            JsonToken.String("терапевтический участок №1"),
            JsonToken.Property("Category"),
            JsonToken.Number("1"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.01.2017"),
            JsonToken.Property("DateEnd"),
            JsonToken.DateTime("null"),
            JsonToken.EndObject,

            JsonToken.StartObject,
            JsonToken.Property("CodeMo"),
            JsonToken.String("750004"),
            JsonToken.Property("Unit"),
            JsonToken.Number("1"),
            JsonToken.Property("District"),
            JsonToken.Number("2"),
            JsonToken.Property("Name"),
            JsonToken.String("терапевтический участок №2"),
            JsonToken.Property("Category"),
            JsonToken.Number("1"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.01.2017"),
            JsonToken.Property("DateEnd"),
            JsonToken.DateTime("01.01.2019"),
            JsonToken.EndObject,

            JsonToken.StartObject,
            JsonToken.Property("CodeMo"),
            JsonToken.String("750004"),
            JsonToken.Property("Unit"),
            JsonToken.Number("1"),
            JsonToken.Property("District"),
            JsonToken.Number("3"),
            JsonToken.Property("Name"),
            JsonToken.String("терапевтический участок №3"),
            JsonToken.Property("Category"),
            JsonToken.Number("1"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.01.2017"),
            JsonToken.Property("DateEnd"),
            JsonToken.DateTime("null"),
            JsonToken.EndObject,

            JsonToken.StartObject,
            JsonToken.Property("CodeMo"),
            JsonToken.String("750004"),
            JsonToken.Property("Unit"),
            JsonToken.Number("1"),
            JsonToken.Property("District"),
            JsonToken.Number("4"),
            JsonToken.Property("Name"),
            JsonToken.String("педиатрический участок №1"),
            JsonToken.Property("Category"),
            JsonToken.Number("2"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.01.2017"),
            JsonToken.Property("DateEnd"),
            JsonToken.DateTime("null"),
            JsonToken.EndObject,

            JsonToken.StartObject,
            JsonToken.Property("CodeMo"),
            JsonToken.String("750004"),
            JsonToken.Property("Unit"),
            JsonToken.Number("1"),
            JsonToken.Property("District"),
            JsonToken.Number("5"),
            JsonToken.Property("Name"),
            JsonToken.String("педиатрический участок №2"),
            JsonToken.Property("Category"),
            JsonToken.Number("2"),
            JsonToken.Property("DateBegin"),
            JsonToken.DateTime("01.01.2017"),
            JsonToken.Property("DateEnd"),
            JsonToken.DateTime("null"),
            JsonToken.EndObject,

            JsonToken.EndArray
        }), actual);

    }
}
