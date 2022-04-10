namespace catalog.Core.SharedKernel.Base.Domain;

public interface IEntityId : IEquatable<IEntityId>
{
    public long Value { get; }
}
