namespace catalog.Infrastructure.Cache;

/// <summary>
/// Служба таймера
/// </summary>
public interface ITimeout
{
    /// <summary>
    /// Запустить делегат по таймеру
    /// </summary>
    /// <param name="action">Делегат</param>
    void StartTimer(Action action);
}
