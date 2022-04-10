namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlRegion
{
    public string RegionCode { get; set; }
    public DateTime Version { get; set; }
    public long Size { get; set; }
    public List<XmlObject> Objects { get; set; }
}
