namespace CustomDialogLibrary.Entities;

public abstract class FileEntityModel(FileSystemInfo fileSystemInfo)
{
    private FileSystemInfo FileSystemInfo { get; } = fileSystemInfo;
    public string Title => FileSystemInfo.Name;
    public string FullPath => FileSystemInfo.FullName;
    public string Extension => FileSystemInfo.Extension;
    public DateTime LastAccessTime => FileSystemInfo.LastAccessTime;
    public DateTime CreationTime => FileSystemInfo.CreationTime;

    public string Size { get; } = fileSystemInfo switch
    {
        FileInfo fileInfo => fileInfo.Length + " bytes",
        _ => ""
    };
}