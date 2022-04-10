namespace catalog.Core.SharedKernel.Base.Domain;

/// <summary>
/// Сущность домена
/// <para>Базовый класс</para>
/// </summary>
public abstract class Entity<TId> where TId : IEntityId
{
    public virtual TId Id { get; protected set; }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return CompareValues(this, (Entity<TId>)obj);
    }

    public bool Equals(Entity<TId> obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return CompareValues(this, obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Сравнить значения
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool CompareValues(Entity<TId> value1, Entity<TId> value2)
    {
        return value1.Id.Equals(value2.Id);
    }
}
