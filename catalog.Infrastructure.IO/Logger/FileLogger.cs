using catalog.Core.Application.Common.Service;
using catalog.Infrastructure.Utility.FileSystem;

namespace catalog.Infrastructure.IO.Logger;

/// <summary>
/// Логгер вывод в файл
/// </summary>
public class FileLogger : ILogger
{
    #region Fields
    IFileService fileService;
    IAppLocation appLocation;

    string fileName;
    #endregion

    public FileLogger(IFileService fileService, IAppLocation appLocation)
    {
        this.fileService = fileService;
        this.appLocation = appLocation;

        fileName = string.Format("{0}_{1}.txt", "Log", DateTime.Now.ToString("yyyyMMdd"));
    }

    public void Debug(string message)
    {
        Log("Debug", message, ConsoleColor.Yellow);
    }

    public void Error(string message)
    {
        Log("Error", message, ConsoleColor.Red);
    }

    public void Fatal(string message)
    {
        Log("Fatal", message, ConsoleColor.DarkRed);
    }

    public void Info(string message)
    {
        Log("Info", message, ConsoleColor.Green);
    }

    private void Log(string level, string message, ConsoleColor color)
    {
        string logStr = string.Format(string.Format("{0}  {1}\t{2}", DateTime.Now.ToString("hh:mm:ss:fff"), level, message));
        fileService.AddTextToFile(fileName, appLocation.LogPath, logStr);
    }
}
