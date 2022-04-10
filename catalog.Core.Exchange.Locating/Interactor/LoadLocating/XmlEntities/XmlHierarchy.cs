namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlHierarchy
{
    public XmlHierarchy()
    {
    }

    public long Id { get; set; }
    public long ParentId { get; set; }

    public DateTime? EndDate { get; set; }
    public int IsActive { get; set; }

    //public DateTime? UpdateDate { get; set; }
    //public DateTime? StartDate { get; set; }
}
