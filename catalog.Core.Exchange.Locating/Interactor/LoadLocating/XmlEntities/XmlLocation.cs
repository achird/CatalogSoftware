using catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlLocation : XmlObject
{
    public XmlLocation()
    {
    }

    private readonly XmlLocationName name = new();
    public override string Name => name.Naming();
    public override string ProperName => name.ProperNaming();

    public string ObjectType
    {
        set
        {
            name.ObjectType = value;
        }
    }
    public string ObjectName
    {
        set
        {
            name.ObjectName = value;
        }
    }
}
