namespace catalog.Core.SharedKernel.Base.Domain;

/// <summary>
/// Корень агрегата
/// </summary>
public class AggregateRoot<TId> : Entity<TId> where TId : IEntityId
{
}
