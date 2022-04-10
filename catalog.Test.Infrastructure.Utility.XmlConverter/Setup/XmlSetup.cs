using catalog.Infrastructure.Utility.XmlConverter;
using System.IO;

namespace catalog.Test.Infrastructure.Utility.XmlConverter.Setup;

public static class XmlSetup
{
    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static Stream GenerateXmlCollection()
    {
        return GenerateStreamFromString(
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<root>"
        + "  <rec CODE_MO=\"750004\" NOM_PODR=\"1\" DEPTH=\"1\" NAME_DEPTH=\"терапевтический участок №1\" KATEG=\"1\" DATE_B=\"01.01.2017\" DATE_E=\"\" />"
        + "  <rec CODE_MO=\"750004\" NOM_PODR=\"1\" DEPTH=\"2\" NAME_DEPTH=\"терапевтический участок №2\" KATEG=\"1\" DATE_B=\"01.01.2017\" DATE_E=\"01.01.2019\" />"
        + "  <rec CODE_MO=\"750004\" NOM_PODR=\"1\" DEPTH=\"3\" NAME_DEPTH=\"терапевтический участок №3\" KATEG=\"1\" DATE_B=\"01.01.2017\" DATE_E=\"\" />"
        + "  <rec CODE_MO=\"750004\" NOM_PODR=\"1\" DEPTH=\"4\" NAME_DEPTH=\"педиатрический участок №1\" KATEG=\"2\" DATE_B=\"01.01.2017\" DATE_E=\"\" />"
        + "  <rec CODE_MO=\"750004\" NOM_PODR=\"1\" DEPTH=\"5\" NAME_DEPTH=\"педиатрический участок №2\" KATEG=\"2\" DATE_B=\"01.01.2017\" DATE_E=\"\" />"
        + "</root>");
    }
    public static ISchema GetXmlCollectionSchema()
    {
        return new Schema()
            .Array("root")
            .Object("rec")
                .PropertyString("CODE_MO", "CodeMo")
                .PropertyNumber("NOM_PODR", "Unit")
                .PropertyNumber("DEPTH", "District")
                .PropertyString("NAME_DEPTH", "Name")
                .PropertyNumber("KATEG", "Category")
                .PropertyDateTime("DATE_B", "DateBegin")
                .PropertyDateTime("DATE_E", "DateEnd");
    }

    public static Stream GenerateXmlClass()
    {
        return GenerateStreamFromString(
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<packet>"
        + "  <zglv version=\"1.1\">"
        + "    <type></type>"
        + "    <date>03.10.2019</date>"
        + "  </zglv>"
        + "  <entry KOD=\"0\">"
        + "    <FNAME>Не требуется изготовление временного свидетельства</FNAME>"
        + "    <DATEBEG>01.10.2019</DATEBEG>"
        + "    <DATEEND/>"
        + "  </entry>"
        + "  <entry KOD=\"1\">"
        + "    <FNAME>Временное свидетельство на бумажном бланке</FNAME>"
        + "    <DATEBEG>01.05.2011</DATEBEG>"
        + "    <DATEEND/>"
        + "  </entry>"
        + "  <entry KOD=\"2\">"
        + "    <FNAME>Временное свидетельство в электронной форме</FNAME>"
        + "    <DATEBEG>01.06.2019</DATEBEG>"
        + "    <DATEEND/>"
        + "  </entry>"
        + "</packet>");
    }

    public static ISchema GetXmlClassSchema()
    {
        return new Schema()
            .Add("packet")
            .Object("zglv")
                .PropertyNumber("version", "Version")
                .PropertyDateTime("date", "UpdateDate")
            .End()
            .Object("entry")
                .PropertyNumber("KOD", "Code")
                .PropertyString("FNAME", "Name")
                .PropertyDateTime("DATEBEG", "DateBegin")
                .PropertyDateTime("DATEEND", "DateEnd");
    }

    public static Stream GenerateXmlComplexClass()
    {
        return GenerateStreamFromString(
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
        + "<packets>"
        + "  <packet>"
        + "    <zglv version=\"1.1\">"
        + "      <type></type>"
        + "      <date>01.10.2019</date>"
        + "    </zglv>"
        + "    <entry KOD=\"0\">"
        + "      <value CODE=\"000\">"
        + "        <NAME>Значение 000</NAME>"
        + "      </value>"
        + "      <FNAME>Не требуется изготовление временного свидетельства</FNAME>"
        + "      <DATEBEG>01.10.2019</DATEBEG>"
        + "      <DATEEND/>"
        + "    </entry>"
        + "  </packet>"
        + "  <packet>"
        + "    <zglv version=\"1.2\">"
        + "      <type></type>"
        + "      <date>01.10.2020</date>"
        + "    </zglv>"
        + "    <entry KOD=\"1\">"
        + "      <value CODE=\"001\">"
        + "        <NAME>Значение 001</NAME>"
        + "      </value>"
        + "      <FNAME>Временное свидетельство на бумажном бланке</FNAME>"
        + "      <DATEBEG>01.10.2020</DATEBEG>"
        + "      <DATEEND/>"
        + "    </entry>"
        + "  </packet>"
        + "</packets>");
    }

    public static ISchema GetXmlComplexClassSchema()
    {
        return new Schema()
            .Add("packets")
            .Object("packet")
                .Object("zglv", "Header")
                    .PropertyNumber("version", "Version")
                    .PropertyDateTime("date", "UpdateDate")
                .End()
                .Object("entry", "Item")
                    .PropertyNumber("KOD", "Code")
                    .PropertyString("FNAME", "Name")
                    .PropertyDateTime("DATEBEG", "DateBegin")
                    .PropertyDateTime("DATEEND", "DateEnd")
                    .Object("value", "Value")
                        .PropertyString("CODE", "Code")
                        .PropertyString("NAME", "Name");
    }
}
