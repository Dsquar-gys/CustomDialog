using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;

namespace CustomDialogLibrary.Interfaces;

public interface ISpecificFileViewModel
{
    BodyTemplate? CurrentBodyTemplate { get; }
    ICommand? Command { get; }
    ObservableCollection<StyleSelector> StyleButtons { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm);
}