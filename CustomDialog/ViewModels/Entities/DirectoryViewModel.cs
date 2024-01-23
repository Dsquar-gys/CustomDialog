using System.IO;

namespace CustomDialog.ViewModels.Entities;

public sealed class DirectoryViewModel : FileEntityViewModel
{
    public DirectoryViewModel(string name) : base(name) => FullName = name;

    public DirectoryViewModel(DirectoryInfo directory) : base(directory.FullName) => FullName = directory.FullName;
}