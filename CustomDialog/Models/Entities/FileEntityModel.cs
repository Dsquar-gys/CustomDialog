using System;
using System.IO;
using CustomDialog.Models.Interfaces;
using CustomDialog.ViewModels;

namespace CustomDialog.Models.Entities;

public abstract class FileEntityModel(FileSystemInfo fileSystemInfo) : ViewModelBase, IImagable
{
    private FileSystemInfo FileSystemInfo { get; } = fileSystemInfo;
    public string Title => FileSystemInfo.Name;
    public string FullPath => FileSystemInfo.FullName;
    public DateTime LastAccessTime => FileSystemInfo.LastAccessTime;
    public DateTime CreationTime => FileSystemInfo.CreationTime;
    public string IconName { get; } = fileSystemInfo switch
    {
        FileInfo => "file",
        DirectoryInfo => "folder",
        _ => ImageHelper.DefaultIconName
    };

    public string Size { get; } = fileSystemInfo switch
    {
        FileInfo fileInfo => fileInfo.Length + " bytes",
        _ => ""
    };
}