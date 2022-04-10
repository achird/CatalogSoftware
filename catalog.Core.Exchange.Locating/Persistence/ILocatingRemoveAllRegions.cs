namespace catalog.Core.Exchange.Locating.Persistence;

/// <summary>
/// Удалить данные всех регионов из хранилища
/// </summary>
public interface ILocatingRemoveAllRegions
{
    /// <summary>
    /// Удалить данные всех регионов из хранилища
    /// </summary>
    public Task RemoveAllAsync();
}
