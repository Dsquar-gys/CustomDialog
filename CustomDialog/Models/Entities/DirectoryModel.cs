using System.IO;
using CustomDialog.Models;

namespace CustomDialog.ViewModels.Entities;

public sealed class DirectoryModel : FileEntityModel, ILoadable
{
    public DirectoryModel(string dirPath, string dirName) : base(dirPath, dirName, "folder") {}

    public DirectoryModel(DirectoryInfo directory) : base(directory.FullName, directory.Name, "folder") {}
}