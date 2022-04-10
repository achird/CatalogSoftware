using catalog.Core.Application.Common.Service;

namespace catalog.Infrastructure.IO.Logger;

/// <summary>
/// Логгер вывод в консоль
/// </summary>
public class ConsoleLogger : ILogger
{
    public ConsoleLogger()
    { }

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
        //string logStr = string.Format("{0:u}\t{1}\t{2}", DateTime.Now.ToShortTimeString(), level, message);

        Console.Write(string.Format("{0}  ", DateTime.Now.ToString("hh:mm:ss:fff")));
        Console.ForegroundColor = color;
        Console.Write(string.Format("{0}\t", level));
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(string.Format("{0}\r\n", message));
    }
}
