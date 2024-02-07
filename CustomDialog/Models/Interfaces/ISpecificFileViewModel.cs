using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CustomDialog.Models.Entities;
using CustomDialog.Views.BodyTemplates;

namespace CustomDialog.Models.Interfaces;

public interface ISpecificFileViewModel
{
    BodyTemplate? CurrentBodyTemplate { get; }
    ICommand? Command { get; }
    ObservableCollection<StyleSelector> StyleButtons { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm);
}