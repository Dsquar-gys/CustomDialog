namespace CustomDialogLibrary.Entities;

/// <summary>
/// Entity, which represents File or Directory
/// </summary>
public abstract class FileEntityModel(FileSystemInfo fileSystemInfo)
{
    public string Name => fileSystemInfo.Name;
    public string FullPath => fileSystemInfo.FullName;
    public string Extension => new(fileSystemInfo.Extension.Skip(1).ToArray());
    public string Type => fileSystemInfo is FileInfo ? "File" : "Directory";
    
    // For DataGridTemplate
    public DateTime LastAccessTime => fileSystemInfo.LastAccessTime;
    public DateTime CreationTime => fileSystemInfo.CreationTime;

    public string Size { get; } = fileSystemInfo switch
    {
        FileInfo fileInfo => fileInfo.Length + " bytes",
        _ => ""
    };
}