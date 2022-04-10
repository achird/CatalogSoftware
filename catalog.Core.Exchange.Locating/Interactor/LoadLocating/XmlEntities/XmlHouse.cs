using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlHouse : XmlObject
{
    public XmlHouse()
    {
        ObjectLevel = 10;
    }

    private readonly XmlHouseName name = new();
    public override string Name => name.Naming();
    public override string ProperName => name.ProperNaming();

    public int? HouseType1
    {
        set
        {
            name.HouseType1 = value;
        }
    }
    public string HouseNumber1
    {
        set
        {
            name.HouseNumber1 = value;
        }
    }
    public int? HouseType2
    {
        set
        {
            name.HouseType2 = value;
        }
    }
    public string HouseNumber2
    {
        set
        {
            name.HouseNumber2 = value;
        }
    }
    public int? HouseType3
    {
        set
        {
            name.HouseType3 = value;
        }
    }
    public string HouseNumber3
    {
        set
        {
            name.HouseNumber3 = value;
        }
    }
}
