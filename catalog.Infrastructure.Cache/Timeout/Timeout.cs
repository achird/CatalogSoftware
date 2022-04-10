namespace catalog.Infrastructure.Cache;

/// <summary>
/// Служба таймера
/// </summary>
public class Timeout : ITimeout
{
    private readonly TimeSpan timeout;
    private Timer timer;

    public Timeout(TimeSpan timeout)
    {
        this.timeout = timeout;
    }

    /// <summary>
    /// Запустить делегат по таймеру
    /// </summary>
    /// <param name="action">Делегат</param>
    public void StartTimer(Action action)
    {
        if (this.action != null) throw new InvalidOperationException("Timeout уже работает");

        this.action = action;
        this.timer = new Timer((obj) => RaiseExpire(), this, timeout, timeout);
    }

    private Action action;

    private void RaiseExpire()
    {
        if (action == null) throw new InvalidOperationException("Действие для Timeout не установлено");

        action();
    }

    ~Timeout()
    {
        if (timer != null)
            timer.Dispose();
    }
}
