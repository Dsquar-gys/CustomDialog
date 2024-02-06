using System.IO;
using CustomDialog.Models.Interfaces;

namespace CustomDialog.Models.Entities;

public sealed class DirectoryModel : FileEntityModel, ILoadable
{
    public DirectoryModel(ISpecificFileViewModel svm, string dirPath, string dirName) : base(svm, dirPath, dirName) {}

    public DirectoryModel(ISpecificFileViewModel svm, DirectoryInfo directory) : base(svm, directory.FullName, directory.Name) {}
}