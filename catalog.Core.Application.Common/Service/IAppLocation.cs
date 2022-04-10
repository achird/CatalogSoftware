namespace catalog.Core.Application.Common.Service;

/// <summary>
/// Расположения приложения
/// </summary>
public interface IAppLocation
{
    string AppPath { get; }
    string CachePath { get; }
    string ExchangePath { get; }
    string LogPath { get; }
    string SourcePath { get; }
}
