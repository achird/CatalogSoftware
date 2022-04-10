namespace catalog.Core.Locating.Application.Persistence.Repository;

/// <summary>
/// Единица работы с хранилищем
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Сохранить изменения
    /// </summary>
    /// <returns></returns>
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
