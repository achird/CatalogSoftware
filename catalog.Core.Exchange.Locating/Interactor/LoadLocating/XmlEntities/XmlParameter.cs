namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public class XmlParameter
{
    //public long Id { get; set; }
    public long ObjectId { get; set; }
    //public long ChangeId { get; set; }
    //public long ChangeIdEnd { get; set; }
    public int Type { get; set; }
    public string Value { get; set; }
    public DateTime? StartDate { get; set; }
}
