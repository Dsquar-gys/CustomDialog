namespace CustomDialogLibrary.Models;

/// <summary>
/// Entity, which represents File or Directory
/// </summary>
public abstract class FileEntityModelBase(FileSystemInfo fileSystemInfo)
{
    public string IconName { get; init; } = string.Empty;
    public string Name => fileSystemInfo.Name;
    public string FullPath => fileSystemInfo.FullName;
    public string Extension => fileSystemInfo.Extension;
    public string Type => fileSystemInfo is FileInfo ? "File" : "Directory";
    
    public DateTime LastAccessTime => fileSystemInfo.LastAccessTime;
    public DateTime CreationTime => fileSystemInfo.CreationTime;

    public string Size { get; } = fileSystemInfo switch
    {
        FileInfo fileInfo => fileInfo.Length + " bytes",
        _ => ""
    };
    
    public bool Hidden => (fileSystemInfo.Attributes & FileAttributes.Hidden) != 0;
}