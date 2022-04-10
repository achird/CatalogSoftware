using System.Text;

namespace catalog.Infrastructure.Utility.XmlConverter.Common;

/// <summary>
/// Путь
/// </summary>
internal class Crumb
{
    private readonly Stack<int> depth = new();
    private readonly StringBuilder path = new();

    public string CurrentPath => path.ToString();
    public int Depth => depth.Count;

    /// <summary>
    /// Добавить элемент к пути
    /// </summary>
    public string Push(string name)
    {
        if (Depth != 0)
        {
            name = $".{name}";
        }
        path.Append(name);
        depth.Push(name.Length);
        return path.ToString();
    }

    /// <summary>
    /// Попытаться удалить элемент пути
    /// </summary>
    public bool TryPop(out string name)
    {
        if (depth.TryPop(out int length))
        {
            name = path.ToString();
            path.Remove(path.Length - length, length);

            return true;
        }

        name = null;
        return false;
    }
}
