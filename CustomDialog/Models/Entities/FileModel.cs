using System.IO;
using CustomDialog.Models.Interfaces;

namespace CustomDialog.Models.Entities;

public sealed class FileModel : FileEntityModel
{
    public FileModel(ISpecificFileViewModel svm, string filePath, string fileName) : base(svm, filePath, fileName) {}
    public FileModel(ISpecificFileViewModel svm, FileInfo file) : base(svm, file.FullName, file.Name) {}
}