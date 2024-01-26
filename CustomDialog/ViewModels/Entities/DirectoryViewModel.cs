using System.IO;
using CustomDialog.Models;

namespace CustomDialog.ViewModels.Entities;

public sealed class DirectoryViewModel : FileEntityViewModel, ILoadable
{
    public DirectoryViewModel(string dirPath, string dirName) : base(dirPath, dirName, "folder") {}

    public DirectoryViewModel(DirectoryInfo directory) : base(directory.FullName, directory.Name, "folder") {}
}