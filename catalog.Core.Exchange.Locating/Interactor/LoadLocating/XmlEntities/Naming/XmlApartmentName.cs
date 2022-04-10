namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

public class XmlApartmentName : XmlNaming
{
    private static readonly Dictionary<int, ObjectType> apartmentTypes = new()
    {
        { 1, new ObjectType() { Name = "Помещение", ShortName = "помещение", Position = TypePosition.Left } },
        { 2, new ObjectType() { Name = "Квартира", ShortName = "кв", Position = TypePosition.Left } },
        { 3, new ObjectType() { Name = "Офис", ShortName = "офис", Position = TypePosition.Left } },
        { 4, new ObjectType() { Name = "Комната", ShortName = "комната", Position = TypePosition.Left } },
        { 5, new ObjectType() { Name = "Рабочий участок", ShortName = "раб.уч.", Position = TypePosition.Left } },
        { 6, new ObjectType() { Name = "Склад", ShortName = "скл.", Position = TypePosition.Left } },
        { 7, new ObjectType() { Name = "Торговый зал", ShortName = "торг.зал", Position = TypePosition.Left } },
        { 8, new ObjectType() { Name = "Цех", ShortName = "цех", Position = TypePosition.Left } },
        { 9, new ObjectType() { Name = "Павильон", ShortName = "пав", Position = TypePosition.Left } },
        { 10, new ObjectType() { Name = "Подвал", ShortName = "подв.", Position = TypePosition.Left } },
        { 11, new ObjectType() { Name = "Котельная", ShortName = "кот.", Position = TypePosition.Left } },
        { 12, new ObjectType() { Name = "Погреб", ShortName = "п-б", Position = TypePosition.Left } },
        { 13, new ObjectType() { Name = "Гараж", ShortName = "г-ж", Position = TypePosition.Left } }
    };

    public int? ApartmentType { get; set; }
    public string ApartmentNumber { get; set; }

    public override string Naming()
    {
        if (ApartmentType is null && string.IsNullOrEmpty(ApartmentNumber) != true)
            return ApartmentNumber;

        if (string.IsNullOrEmpty(ApartmentNumber))
            return default;

        if (apartmentTypes.TryGetValue(ApartmentType ?? 0, out ObjectType objectType))
        {
            return objectType.Position switch
            {
                // правая позиция
                TypePosition.Right => string.Join(" ", ApartmentNumber, objectType.ShortName),
                // в остальных случаях - левая позиция
                _ => string.Join(" ", objectType.ShortName, ApartmentNumber),
            };
        }

        return default;
    }
    public override string ProperNaming()
    {
        return ApartmentNumber;
    }
}
