using catalog.Core.Application.Common.Service;

namespace catalog.Infrastructure.IO.AppLocation;

public class AppLocation : IAppLocation
{
    public AppLocation(string appPath)
    {
        AppPath = appPath;
    }
    /// <summary>
    /// Путь к каталогу приложения
    /// </summary>
    public string AppPath { get; }

    /// <summary>
    /// Путь к каталогу с данными
    /// </summary>
    public string SourcePath => Path.Combine(AppPath, "Source");

    /// <summary>
    /// Путь к каталогу с кэшем данных
    /// </summary>
    public string CachePath => Path.Combine(SourcePath, "Cache");

    /// <summary>
    /// Путь к каталогу с логами
    /// </summary>
    public string LogPath => Path.Combine(SourcePath, "Log");

    /// <summary>
    /// Путь к каталогу 
    /// </summary>
    public string ExchangePath => Path.Combine(SourcePath, "Exchange");

}
