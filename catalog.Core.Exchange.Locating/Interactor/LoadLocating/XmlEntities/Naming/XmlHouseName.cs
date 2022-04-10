namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

public class XmlHouseName : XmlNaming
{
    private static readonly Dictionary<int, ObjectType> houseTypes = new()
    {
        { 1, new ObjectType() { Name = "Владение", ShortName = "влд.", Position = TypePosition.Left } },
        { 2, new ObjectType() { Name = "Дом", ShortName = "д.", Position = TypePosition.Left } },
        { 3, new ObjectType() { Name = "Домовладение", ShortName = "двлд.", Position = TypePosition.Left } },
        { 4, new ObjectType() { Name = "Гараж", ShortName = "г-ж", Position = TypePosition.Left } },
        { 5, new ObjectType() { Name = "Здание", ShortName = "зд.", Position = TypePosition.Left } },
        { 6, new ObjectType() { Name = "Шахта", ShortName = "шахта", Position = TypePosition.Left } },
        { 7, new ObjectType() { Name = "Строение", ShortName = "стр.", Position = TypePosition.Left } },
        { 8, new ObjectType() { Name = "Сооружение", ShortName = "соор.", Position = TypePosition.Left } },
        { 9, new ObjectType() { Name = "Литера", ShortName = "литера", Position = TypePosition.Left } },
        { 10, new ObjectType() { Name = "Корпус", ShortName = "к.", Position = TypePosition.Left } },
        { 11, new ObjectType() { Name = "Подвал", ShortName = "подв.", Position = TypePosition.Left } },
        { 12, new ObjectType() { Name = "Котельная", ShortName = "кот.", Position = TypePosition.Left } },
        { 13, new ObjectType() { Name = "Погреб", ShortName = "п-б", Position = TypePosition.Left } },
        { 14, new ObjectType() { Name = "Объект незавершенного строительства", ShortName = "ОНС", Position = TypePosition.Left } }
    };
    private static readonly Dictionary<int, ObjectType> houseAddTypes = new()
    {
        { 1, new ObjectType() { Name = "Корпус", ShortName = "к.", Position = TypePosition.Left } },
        { 2, new ObjectType() { Name = "Строение", ShortName = "стр.", Position = TypePosition.Left } },
        { 3, new ObjectType() { Name = "Сооружение", ShortName = "соор.", Position = TypePosition.Left } },
        { 4, new ObjectType() { Name = "Литера", ShortName = "литера", Position = TypePosition.Left } }
    };

    public int? HouseType1 { get; set; }
    public string HouseNumber1 { get; set; }
    public int? HouseType2 { get; set; }
    public string HouseNumber2 { get; set; }
    public int? HouseType3 { get; set; }
    public string HouseNumber3 { get; set; }

    private string GetHouseName(Dictionary<int, ObjectType> objectTypes, int? type, string number)
    {
        string name = number;

        if (objectTypes.TryGetValue(type ?? 0, out ObjectType objectType))
        {
            name = objectType.Position switch
            {
                // левая позиция
                TypePosition.Left => string.Join(" ", objectType.ShortName, number),
                // в остальных случаях - правая позиция
                _ => string.Join(" ", number, objectType.ShortName),
            };
        }

        return name;
    }
    public override string Naming()
    {
        List<string> names = new();
        if (HouseType1 is not null && HouseNumber1 is not null)
            names.Add(GetHouseName(houseTypes, HouseType1, HouseNumber1));
        if (HouseType2 is not null && HouseNumber2 is not null)
            names.Add(GetHouseName(houseAddTypes, HouseType2, HouseNumber2));
        if (HouseType3 is not null && HouseNumber3 is not null)
            names.Add(GetHouseName(houseAddTypes, HouseType3, HouseNumber3));
        return names.Count != 0 ? string.Join(" ", names) : string.Empty;
    }
    public override string ProperNaming()
    {
        List<string> names = new();
        if (HouseType1 is not null && HouseNumber1 is not null)
            names.Add(HouseNumber1);
        if (HouseType2 is not null && HouseNumber2 is not null)
            names.Add(HouseNumber2);
        if (HouseType3 is not null && HouseNumber3 is not null)
            names.Add(HouseNumber3);
        return names.Count != 0 ? string.Join("/", names) : string.Empty;
    }
}

