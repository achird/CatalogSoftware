namespace catalog.Core.Application.Common.Service;

/// <summary>
/// Логгер
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Информация
    /// </summary>
    /// <param name="message"></param>
    void Info(string message);

    /// <summary>
    /// Отладка
    /// </summary>
    /// <param name="message"></param>
    void Debug(string message);

    /// <summary>
    /// Ошибка
    /// </summary>
    /// <param name="message"></param>
    void Error(string message);

    /// <summary>
    /// Критическая ошибка
    /// </summary>
    /// <param name="message"></param>
    void Fatal(string message);
}
