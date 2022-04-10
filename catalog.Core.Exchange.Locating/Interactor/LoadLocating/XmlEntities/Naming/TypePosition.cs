namespace catalog.Core.Exchange.Locating.Interactor.LoadLocating.XmlEntities.Naming;

internal enum TypePosition
{
    Left, Right, LeftRight
}

internal class ObjectType
{
    public string Name { get; set; }
    public string ShortName { get; set; }

    public TypePosition Position { get; set; }

    public readonly static ObjectType DefaultLeft = new() { Name = string.Empty, ShortName = string.Empty, Position = TypePosition.Left };
    public readonly static ObjectType DefaultRight = new() { Name = string.Empty, ShortName = string.Empty, Position = TypePosition.Right };
}
