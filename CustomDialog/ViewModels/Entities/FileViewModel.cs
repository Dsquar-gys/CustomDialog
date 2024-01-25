using System.IO;
using Avalonia.Media;

namespace CustomDialog.ViewModels.Entities;

public sealed class FileViewModel : FileEntityViewModel
{
    public FileViewModel(string filePath, string fileName) : base(filePath, fileName) {}
    public FileViewModel(FileInfo file) : base(file.FullName, file.Name) {}
}