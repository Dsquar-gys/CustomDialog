using System.IO;
using System.Windows.Input;
using Avalonia.Controls.Templates;
using CustomDialog.Models.Entities;

namespace CustomDialog.Models.Interfaces;

public interface ISpecificFileViewModel : IImagable
{
    IDataTemplate? LocalDataTemplate { get; }
    ICommand? Command { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm);
}