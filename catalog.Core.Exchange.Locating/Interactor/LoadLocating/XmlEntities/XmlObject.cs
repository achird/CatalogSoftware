namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities;

public abstract class XmlObject
{
    public long Id { get; set; }
    public Guid? Uid { get; set; }
    public long ParentId { get; set; }
    public List<long> Structure { get; set; }
    public long? NextId { get; set; }

    public int ObjectLevel { get; set; }
    public int LocationType => ObjectLevel switch
    {
        1 => 1,
        2 => 2,
        5 => 3,
        6 => 4,
        7 => 5,
        8 => 6,
        13 => 6,
        14 => 6,
        15 => 6,
        16 => 6,
        10 => 7,
        11 => 8,
        _ => 0
    };

    public abstract string Name { get; }
    public abstract string ProperName { get; }
    public string PostCode { get; set; }
    public string PlainCode { get; set; }
    public string Okato { get; set; }

    public IEnumerable<XmlParameter> Parameters { get; set; }
}
