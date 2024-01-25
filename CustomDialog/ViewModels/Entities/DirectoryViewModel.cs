using System.IO;

namespace CustomDialog.ViewModels.Entities;

public sealed class DirectoryViewModel : FileEntityViewModel
{
    public DirectoryViewModel(string title) : base(title) => FullPath = title;

    public DirectoryViewModel(DirectoryInfo directory) : base(directory.FullName) => FullPath = directory.FullName;
}