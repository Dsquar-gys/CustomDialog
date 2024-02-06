using System.IO;

namespace CustomDialog.ViewModels.Entities;

public sealed class FileModel : FileEntityModel
{
    public FileModel(string filePath, string fileName) : base(filePath, fileName, "file") {}
    public FileModel(FileInfo file) : base(file.FullName, file.Name, "file") {}
}