using catalog.Infrastructure.Utility.XmlConverter.Common;
using catalog.Test.Infrastructure.Utility.XmlConverter.Setup;
using System.Collections.Generic;
using System.Xml;
using Xunit;

namespace catalog.Test.Infrastructure.Utility.XmlConverter;

public class XmlTokenReaderTests
{
    (List<XmlToken>, Stack<XmlToken>) ReadXmlToken(XmlTokenReader tokenReader)
    {
        var tokens = new List<XmlToken>();
        var stack = new Stack<XmlToken>();
        while (tokenReader.Read(out XmlToken token))
        {
            if (token.TokenType == XmlTokenType.StartElement)
            {
                stack.Push(token);
            }
            if (token.TokenType == XmlTokenType.EndElement && stack.TryPeek(out XmlToken temp) && temp.Value == token.Value)
            {
                stack.Pop();
            }
            tokens.Add(token);
        }
        return (tokens, stack);
    }

    [Fact]
    public void TestXmlToken()
    {
        using var xmlReaderClass = XmlReader.Create(XmlSetup.GenerateXmlClass());
        var tokenReader = new XmlTokenReader(xmlReaderClass);
        var (tokens, stack) = ReadXmlToken(tokenReader);

        //Ожидается коллекция из 47 элемента
        Assert.Equal(47, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);

        using var xmlReaderCollection = XmlReader.Create(XmlSetup.GenerateXmlCollection());
        tokenReader = new XmlTokenReader(xmlReaderCollection);
        (tokens, stack) = ReadXmlToken(tokenReader);

        //Ожидается коллекция из 82 элементов
        Assert.Equal(82, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);

        using var xmlReaderComplexClass = XmlReader.Create(XmlSetup.GenerateXmlComplexClass());
        tokenReader = new XmlTokenReader(xmlReaderComplexClass);
        (tokens, stack) = ReadXmlToken(tokenReader);

        //Ожидается коллекция из 62 элементов
        Assert.Equal(62, tokens.Count);
        //Ожидается пустая коллекция
        Assert.Empty(stack);
    }
}
