using catalog.Core.Application.Common.Service;
using catalog.Infrastructure.Utility.FileSystem;

namespace catalog.Infrastructure.IO.Logger;

/// <summary>
/// Логгер вывод в файл и на консоль
/// </summary>
public class ConsoleFileLogger : ILogger
{
    #region Fields
    ConsoleLogger consoleLogger;
    FileLogger fileLogger;
    #endregion

    public ConsoleFileLogger(IFileService fileService, IAppLocation appLocation)
    {
        consoleLogger = new ConsoleLogger();
        fileLogger = new FileLogger(fileService, appLocation);
    }

    public void Debug(string message)
    {
        consoleLogger.Debug(message);
        fileLogger.Debug(message);
    }

    public void Error(string message)
    {
        consoleLogger.Error(message);
        fileLogger.Error(message);
    }

    public void Fatal(string message)
    {
        consoleLogger.Fatal(message);
        fileLogger.Fatal(message);
    }

    public void Info(string message)
    {
        consoleLogger.Info(message);
        fileLogger.Info(message);
    }
}
