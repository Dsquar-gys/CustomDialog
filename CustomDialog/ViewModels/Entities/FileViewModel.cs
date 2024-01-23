using System.IO;

namespace CustomDialog.ViewModels.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public FileViewModel(string fileName) : base(fileName) => FullName = fileName;
    public FileViewModel(FileInfo fileName) : base(fileName.FullName) => FullName = fileName.FullName;
}