namespace CustomDialogLibrary.Entities;

public abstract class FileEntityModel(FileSystemInfo fileSystemInfo)
{
    public string Title => fileSystemInfo.Name;
    public string FullPath => fileSystemInfo.FullName;
    public string Extension => fileSystemInfo.Extension;
    
    // For DataGridTemplate
    public DateTime LastAccessTime => fileSystemInfo.LastAccessTime;
    public DateTime CreationTime => fileSystemInfo.CreationTime;

    public string Size { get; } = fileSystemInfo switch
    {
        FileInfo fileInfo => fileInfo.Length + " bytes",
        _ => ""
    };
}