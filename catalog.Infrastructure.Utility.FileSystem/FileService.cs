using System.IO.Compression;

namespace catalog.Infrastructure.Utility.FileSystem;

public class FileService : IFileService
{
    public FileService()
    { }

    /// <summary>
    /// Проверить существует ли директория на диске
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public bool DirectoryExists(string dirPath)
    {
        return Directory.Exists(dirPath);
    }

    /// <summary>
    /// Создать директорию с индексацией имени
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns>Полный путь созданной директории</returns>
    public string CreateDirectory(string dirPath)
    {
        int i = 1;
        string tempPath = dirPath;
        while (Directory.Exists(tempPath))
        {
            tempPath = string.Format("{0}_{1}", Path.GetDirectoryName(dirPath), i++);
        }

        Directory.CreateDirectory(tempPath);
        return string.Format(@"{0}\", tempPath);
    }

    public string CreateDictionaryIfNotExists(string dirPath)
    {
        if (Directory.Exists(dirPath) != true)
        {
            Directory.CreateDirectory(dirPath);
        }
        return dirPath;
    }

    /// <summary>
    /// Получить список имен файлов в директории
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public IList<string> GetFileNamesFromDirectory(string dirPath)
    {
        DirectoryInfo di = new DirectoryInfo(dirPath);
        return di.GetFiles().Select(a => a.Name).ToList();
    }

    /// <summary>
    /// Получить список имен файлов в директории
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public IList<string> GetFileNamesFromDirectory(string dirPath, string searchPattern)
    {
        DirectoryInfo di = new DirectoryInfo(dirPath);
        return di.GetFiles(searchPattern).Select(a => a.Name).ToList();
    }

    /// <summary>
    /// Создать файл с индексацией имени
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public string CreateFile(string fileName, string dirPath)
    {
        int i = 1;
        string tempFileName = Path.Combine(dirPath, fileName);
        while (File.Exists(tempFileName))
        {
            FileInfo fi = new FileInfo(Path.Combine(dirPath, fileName));
            tempFileName = Path.Combine(dirPath, string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(fi.Name), i++, fi.Extension));
        }

        File.Create(tempFileName);
        return tempFileName;
    }

    /// <summary>
    /// Добавить текст к текстовому файлу. Создать файл, если его не существует
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dirPath"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public void AddTextToFile(string fileName, string dirPath, string text)
    {
        FileInfo fi = new FileInfo(Path.Combine(dirPath, fileName));

        using (StreamWriter sw = new StreamWriter(fi.FullName, true))
        {
            sw.WriteLine(text);
        }
    }

    /// <summary>
    /// Копировать файл с индексацией имени
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sourceDir"></param>
    /// <param name="destDir"></param>
    public string CopyFile(string fileName, string sourceDir, string destDir)
    {
        int i = 1;
        string tempFileName = Path.Combine(destDir, fileName);
        while (File.Exists(tempFileName))
        {
            FileInfo fi = new FileInfo(Path.Combine(destDir, fileName));
            tempFileName = Path.Combine(destDir, string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(fi.Name), i++, fi.Extension));
        }

        File.Copy(Path.Combine(sourceDir, fileName), tempFileName, true);
        return tempFileName;
    }

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sourceDir"></param>
    public void DeleteFile(string fileName, string sourceDir)
    {
        File.Delete(Path.Combine(sourceDir, fileName));
    }

    /// <summary>
    /// Распаковать архив в каталог
    /// </summary>
    /// <param name="zipFileName"></param>
    /// <param name="dirPath"></param>
    public void ExtractZipFile(string zipFileName, string dirPath)
    {
        ZipFile.ExtractToDirectory(zipFileName, dirPath);
    }

    /// <summary>
    /// Запаковать архив в каталог
    /// </summary>
    /// <param name="zipFileName"></param>
    /// <param name="dirPath"></param>
    public void PackZipFile(string zipFileName, string dirPath)
    {
        ZipFile.CreateFromDirectory(dirPath, zipFileName);
    }

}
