using System.IO;

namespace CustomDialog.ViewModels.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public FileViewModel(string filePath, string fileName) : base(filePath, fileName, "file") {}
    public FileViewModel(FileInfo file) : base(file.FullName, file.Name, "file") {}
}