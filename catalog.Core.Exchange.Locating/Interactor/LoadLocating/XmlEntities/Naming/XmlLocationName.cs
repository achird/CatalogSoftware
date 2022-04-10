using System.Collections.Generic;

namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

public class XmlLocationName : XmlNaming
{
    private static readonly Dictionary<string, ObjectType> objectTypes = new()
    {
        { "Аобл", new ObjectType() { Name = "Автономная область", ShortName = "Аобл", Position = TypePosition.Right } },
        { "АО", new ObjectType() { Name = "Автономный округ", ShortName = "АО", Position = TypePosition.Right } },
        { "кв-л", new ObjectType() { Name = "Квартал", ShortName = "кв-л", Position = TypePosition.Right } },
        { "км", new ObjectType() { Name = "Километр", ShortName = "км", Position = TypePosition.Right } },
        { "край", new ObjectType() { Name = "Край", ShortName = "край", Position = TypePosition.Right } },
        { "мкр", new ObjectType() { Name = "Микрорайон", ShortName = "мкр", Position = TypePosition.Right } },
        { "мкр.", new ObjectType() { Name = "Микрорайон", ShortName = "мкр.", Position = TypePosition.Right } },
        { "мост", new ObjectType() { Name = "Мост", ShortName = "мост", Position = TypePosition.Right } },
        { "наб", new ObjectType() { Name = "Набережная", ShortName = "наб", Position = TypePosition.Right } },
        { "наб.", new ObjectType() { Name = "Набережная", ShortName = "наб.", Position = TypePosition.Right } },
        { "обл", new ObjectType() { Name = "Область", ShortName = "обл", Position = TypePosition.Right } },
        { "обл.", new ObjectType() { Name = "Область", ShortName = "обл.", Position = TypePosition.Right } },
        { "округ", new ObjectType() { Name = "Округ", ShortName = "округ", Position = TypePosition.Right } },
        { "ост-в", new ObjectType() { Name = "Остров", ShortName = "ост-в", Position = TypePosition.Right } },
        { "остров", new ObjectType() { Name = "Остров", ShortName = "остров", Position = TypePosition.Right } },
        { "пер-д", new ObjectType() { Name = "Переезд", ShortName = "пер-д", Position = TypePosition.Right } },
        { "переезд", new ObjectType() { Name = "Переезд", ShortName = "переезд", Position = TypePosition.Right } },
        { "пр-д", new ObjectType() { Name = "Проезд", ShortName = "пр-д", Position = TypePosition.Right } },
        { "проезд", new ObjectType() { Name = "Проезд", ShortName = "проезд", Position = TypePosition.Right } },
        { "пр-кт", new ObjectType() { Name = "Проспект", ShortName = "пр-кт", Position = TypePosition.Right } },
        { "р-н", new ObjectType() { Name = "Район", ShortName = "р-н", Position = TypePosition.Right } },
        { "ряд", new ObjectType() { Name = "Ряд(ы)", ShortName = "ряд", Position = TypePosition.Right } },
        { "с/т", new ObjectType() { Name = "Садовое товарищество", ShortName = "с/т", Position = TypePosition.Right } },
        { "с/а", new ObjectType() { Name = "Сельская администрация", ShortName = "с/а", Position = TypePosition.Right } },
        { "с/о", new ObjectType() { Name = "Сельский округ", ShortName = "с/о", Position = TypePosition.Right } },
        { "с/мо", new ObjectType() { Name = "Сельское муницип.образование", ShortName = "с/мо", Position = TypePosition.Right } },
        { "с.п.", new ObjectType() { Name = "Сельское поселение", ShortName = "с.п.", Position = TypePosition.Right } },
        { "с/п", new ObjectType() { Name = "Сельское поселение", ShortName = "с/п", Position = TypePosition.Right } },
        { "с/с", new ObjectType() { Name = "Сельсовет", ShortName = "с/с", Position = TypePosition.Right } },
        { "у", new ObjectType() { Name = "Улус", ShortName = "у", Position = TypePosition.Right } },
        { "у.", new ObjectType() { Name = "Улус", ShortName = "у.", Position = TypePosition.Right } },

        { "аул", new ObjectType() { Name = "Аул", ShortName = "аул", Position = TypePosition.Left } },
        { "гск", new ObjectType() { Name = "Гаражно-строительный кооператив", ShortName = "гск", Position = TypePosition.Left } },
        { "г", new ObjectType() { Name = "Город", ShortName = "г", Position = TypePosition.Left } },
        { "г.", new ObjectType() { Name = "Город", ShortName = "г.", Position = TypePosition.Left } },
        { "г-к", new ObjectType() { Name = "Городок", ShortName = "г-к", Position = TypePosition.Left } },
        { "дп", new ObjectType() { Name = "Дачный поселок", ShortName = "дп", Position = TypePosition.Left } },
        { "дп.", new ObjectType() { Name = "Дачный поселок", ShortName = "дп.", Position = TypePosition.Left } },
        { "д", new ObjectType() { Name = "Деревня", ShortName = "д", Position = TypePosition.Left } },
        { "д.", new ObjectType() { Name = "Деревня", ShortName = "д.", Position = TypePosition.Left } },
        { "дор", new ObjectType() { Name = "Дорога", ShortName = "дор", Position = TypePosition.Left } },
        { "дор.", new ObjectType() { Name = "Дорога", ShortName = "дор.", Position = TypePosition.Left } },
        { "массив", new ObjectType() { Name = "Массив", ShortName = "массив", Position = TypePosition.Left } },
        { "нп", new ObjectType() { Name = "Населенный пункт", ShortName = "нп", Position = TypePosition.Left } },
        { "нп.", new ObjectType() { Name = "Населенный пункт", ShortName = "нп.", Position = TypePosition.Left } },
        { "парк", new ObjectType() { Name = "Парк", ShortName = "парк", Position = TypePosition.Left } },
        { "п", new ObjectType() { Name = "Поселок", ShortName = "п", Position = TypePosition.Left } },
        { "п.", new ObjectType() { Name = "Поселок", ShortName = "п.", Position = TypePosition.Left } },
        { "пгт", new ObjectType() { Name = "Поселок городского типа", ShortName = "пгт", Position = TypePosition.Left } },
        { "пгт.", new ObjectType() { Name = "Поселок городского типа", ShortName = "пгт.", Position = TypePosition.Left } },
        { "п/ст", new ObjectType() { Name = "Поселок и(при) станция(и)", ShortName = "п/ст", Position = TypePosition.Left } },
        { "рп", new ObjectType() { Name = "Рабочий поселок", ShortName = "рп", Position = TypePosition.Left } },
        { "Респ", new ObjectType() { Name = "Республика", ShortName = "Респ", Position = TypePosition.Left } },
        { "респ.", new ObjectType() { Name = "Республика", ShortName = "респ.", Position = TypePosition.Left } },
        { "снт", new ObjectType() { Name = "Садовое товарищество", ShortName = "снт", Position = TypePosition.Left } },
        { "с", new ObjectType() { Name = "Село", ShortName = "с", Position = TypePosition.Left } },
        { "с.", new ObjectType() { Name = "Село", ShortName = "с.", Position = TypePosition.Left } },
        { "ст-ца", new ObjectType() { Name = "Станица", ShortName = "ст-ца", Position = TypePosition.Left } },
        { "тер", new ObjectType() { Name = "Территория", ShortName = "тер", Position = TypePosition.Left } },
        { "тер.", new ObjectType() { Name = "Территория", ShortName = "тер.", Position = TypePosition.Left } },
        { "тер.ф.х.", new ObjectType() { Name = "Территория ФХ", ShortName = "тер.ф.х.", Position = TypePosition.Left } },
        { "тер.ДНТ", new ObjectType() { Name = "Территория ДНТ", ShortName = "тер.ДНТ", Position = TypePosition.Left } },
        { "тер.СНТ", new ObjectType() { Name = "Территория СНТ", ShortName = "тер.СНТ", Position = TypePosition.Left } },
        { "тер.ГСК", new ObjectType() { Name = "Территория ГСК", ShortName = "тер.ГСК", Position = TypePosition.Left } },
        { "тер.ДНО", new ObjectType() { Name = "Территория ДНО", ShortName = "тер.ДНО", Position = TypePosition.Left } },
        { "тер.ДНП", new ObjectType() { Name = "Территория ДНП", ShortName = "тер.ДНП", Position = TypePosition.Left } },
        { "тер.ДПК", new ObjectType() { Name = "Территория ДПК", ShortName = "тер.ДПК", Position = TypePosition.Left } },
        { "тер.ОНО", new ObjectType() { Name = "Территория ОНО", ShortName = "тер.ОНО", Position = TypePosition.Left } },
        { "тер.ОНП", new ObjectType() { Name = "Территория ОНП", ShortName = "тер.ОНП", Position = TypePosition.Left } },
        { "тер.ОНТ", new ObjectType() { Name = "Территория ОНТ", ShortName = "тер.ОНТ", Position = TypePosition.Left } },
        { "тер.ОПК", new ObjectType() { Name = "Территория ОПК", ShortName = "тер.ОПК", Position = TypePosition.Left } },
        { "тер.ПК", new ObjectType() { Name = "Территория ПК", ShortName = "тер.ПК", Position = TypePosition.Left } },
        { "тер.СНО", new ObjectType() { Name = "Территория СНО", ShortName = "тер.СНО", Position = TypePosition.Left } },
        { "тер.СНП", new ObjectType() { Name = "Территория СНП", ShortName = "тер.СНП", Position = TypePosition.Left } },
        { "тер.СОСН", new ObjectType() { Name = "Территория СОСН", ShortName = "тер.СОСН", Position = TypePosition.Left } },
        { "тер.СПК", new ObjectType() { Name = "Территория СПК", ShortName = "тер.СПК", Position = TypePosition.Left } },
        { "тер.ТСЖ", new ObjectType() { Name = "Территория ТСЖ", ShortName = "тер.ТСЖ", Position = TypePosition.Left } },
        { "тер.ТСН", new ObjectType() { Name = "Территория ТСН", ShortName = "тер.ТСН", Position = TypePosition.Left } },
        { "тер. ДНТ", new ObjectType() { Name = "Территория ДНТ", ShortName = "тер. ДНТ", Position = TypePosition.Left } },
        { "тер. СНТ", new ObjectType() { Name = "Территория СНТ", ShortName = "тер. СНТ", Position = TypePosition.Left } },
        { "тер. ГСК", new ObjectType() { Name = "Территория ГСК", ShortName = "тер. ГСК", Position = TypePosition.Left } },
        { "тер. ДНО", new ObjectType() { Name = "Территория ДНО", ShortName = "тер. ДНО", Position = TypePosition.Left } },
        { "тер. ДНП", new ObjectType() { Name = "Территория ДНП", ShortName = "тер. ДНП", Position = TypePosition.Left } },
        { "тер. ДПК", new ObjectType() { Name = "Территория ДПК", ShortName = "тер. ДПК", Position = TypePosition.Left } },
        { "тер. ОНО", new ObjectType() { Name = "Территория ОНО", ShortName = "тер. ОНО", Position = TypePosition.Left } },
        { "тер. ОНП", new ObjectType() { Name = "Территория ОНП", ShortName = "тер. ОНП", Position = TypePosition.Left } },
        { "тер. ОНТ", new ObjectType() { Name = "Территория ОНТ", ShortName = "тер. ОНТ", Position = TypePosition.Left } },
        { "тер. ОПК", new ObjectType() { Name = "Территория ОПК", ShortName = "тер. ОПК", Position = TypePosition.Left } },
        { "тер. ПК", new ObjectType() { Name = "Территория ПК", ShortName = "тер. ПК", Position = TypePosition.Left } },
        { "тер. СНО", new ObjectType() { Name = "Территория СНО", ShortName = "тер. СНО", Position = TypePosition.Left } },
        { "тер. СНП", new ObjectType() { Name = "Территория СНП", ShortName = "тер. СНП", Position = TypePosition.Left } },
        { "тер. СОСН", new ObjectType() { Name = "Территория СОСН", ShortName = "тер. СОСН", Position = TypePosition.Left } },
        { "тер. СПК", new ObjectType() { Name = "Территория СПК", ShortName = "тер. СПК", Position = TypePosition.Left } },
        { "тер. ТСЖ", new ObjectType() { Name = "Территория ТСЖ", ShortName = "тер. ТСЖ", Position = TypePosition.Left } },
        { "тер. ТСН", new ObjectType() { Name = "Территория ТСН", ShortName = "тер. ТСН", Position = TypePosition.Left } },
        { "ул", new ObjectType() { Name = "Улица", ShortName = "ул", Position = TypePosition.Left } },
        { "ул.", new ObjectType() { Name = "Улица", ShortName = "ул.", Position = TypePosition.Left } },
        { "х", new ObjectType() { Name = "Хутор", ShortName = "х", Position = TypePosition.Left } },
        { "х.", new ObjectType() { Name = "Хутор", ShortName = "х.", Position = TypePosition.Left } },

        { "б-р", new ObjectType() { Name = "Бульвар", ShortName = "б-р", Position = TypePosition.LeftRight } },
        { "пл", new ObjectType() { Name = "Площадь", ShortName = "пл", Position = TypePosition.LeftRight } },
        { "пл.", new ObjectType() { Name = "Площадь", ShortName = "пл.", Position = TypePosition.LeftRight } },
        { "пер", new ObjectType() { Name = "Переулок", ShortName = "пер", Position = TypePosition.LeftRight } },
        { "пер.", new ObjectType() { Name = "Переулок", ShortName = "пер.", Position = TypePosition.LeftRight } }
    };

    public string ObjectType { get; set; }
    public string ObjectName { get; set; }

    private TypePosition GetObjectNamePosition()
    {
        if (objectTypes.TryGetValue(ObjectType, out ObjectType objectType))
        {
            switch (objectType.Position)
            {
                case TypePosition.LeftRight:
                    if (ObjectName.EndsWith("а") ||
                        ObjectName.EndsWith("о") ||
                        ObjectName.EndsWith("и") ||
                        ObjectName.EndsWith("ы"))
                        return TypePosition.Left;
                    if (ObjectName.EndsWith("й") ||
                        ObjectName.EndsWith("я"))
                        return TypePosition.Right;
                    break;
                case TypePosition.Left:
                case TypePosition.Right:
                    return objectType.Position;
            }
        }

        return TypePosition.Right;
    }

    public override string Naming()
    {
        if (string.IsNullOrWhiteSpace(ObjectType) && string.IsNullOrEmpty(ObjectName) != true)
            return ObjectName;

        if (string.IsNullOrEmpty(ObjectName))
            return default;

        return GetObjectNamePosition() switch
        {
            // левая позиция
            TypePosition.Left => string.Join(" ", ObjectType, ObjectName),
            // в остальных случаях - правая позиция
            _ => string.Join(" ", ObjectName, ObjectType),
        };
    }
    public override string ProperNaming()
    {
        return ObjectName;
    }
}
