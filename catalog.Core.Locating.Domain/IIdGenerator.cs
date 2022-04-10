using catalog.Core.SharedKernel.Base.Domain;

namespace catalog.Core.Locating.Domain;

public interface IIdGenerator
{
    T NewId<T>() where T : IEntityId;
}
