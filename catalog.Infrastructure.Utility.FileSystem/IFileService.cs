namespace catalog.Infrastructure.Utility.FileSystem;

public interface IFileService
{
    void AddTextToFile(string fileName, string dirPath, string text);
    string CopyFile(string fileName, string sourceDir, string destDir);
    bool DirectoryExists(string dirPath);
    string CreateDirectory(string dirPath);
    string CreateDictionaryIfNotExists(string dirPath);
    string CreateFile(string fileName, string dirPath);
    void DeleteFile(string fileName, string sourceDir);
    void ExtractZipFile(string zipFileName, string dirPath);
    IList<string> GetFileNamesFromDirectory(string dirPath);
    IList<string> GetFileNamesFromDirectory(string dirPath, string searchPattern);
    void PackZipFile(string zipFileName, string dirPath);
}
