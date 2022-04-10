namespace catalog.Core.SharedKernel.Base.Domain;

public abstract class EntityId<T> : ValueObject<T>, IEntityId where T : EntityId<T>
{
    public long Value { get; protected set; }

    public static implicit operator long(EntityId<T> id)
    {
        if (id is null)
            return default;

        return id.Value;
    }

    public bool Equals(IEntityId? other)
    {
        return this == other as T;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
