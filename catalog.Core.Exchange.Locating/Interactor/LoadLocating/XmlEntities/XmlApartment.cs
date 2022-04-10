using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlApartment : XmlObject
{
    public XmlApartment()
    {
        ObjectLevel = 11;
    }

    private readonly XmlApartmentName name = new();
    public override string Name => name.Naming();
    public override string ProperName => name.ProperNaming();

    public int? ApartmentType
    {
        set
        {
            name.ApartmentType = value;
        }
    }
    public string ApartmentNumber
    {
        set
        {
            name.ApartmentNumber = value;
        }
    }
}
