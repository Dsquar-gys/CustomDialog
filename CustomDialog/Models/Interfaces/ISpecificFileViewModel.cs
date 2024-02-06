using System.IO;
using System.Windows.Input;
using Avalonia.Controls.Templates;
using CustomDialog.Models.Entities;

namespace CustomDialog.Models.Interfaces;

public interface ISpecificFileViewModel : IImagable
{
    IDataTemplate? LocalDataTemplate { get; }
    new string IconName { get; }
    string Text { get; }
    ICommand? Command { get; }
    FileSystemInfo FileInfo { get; }
    string? Size { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm);
}