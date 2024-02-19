using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;

namespace CustomDialogLibrary.Interfaces;

public interface ISpecificFileViewModel
{
    /// <summary>
    /// <see cref="BodyTemplate"/> that is selected currently
    /// </summary>
    BodyTemplate? CurrentBodyTemplate { get; }
    
    /// <summary>
    /// Current style for body
    /// </summary>
    StyleSelector? SelectedStyle { get; set; }
    ICommand? Command { get; }
    
    /// <summary>
    /// Collection of available body styles
    /// </summary>
    ObservableCollection<StyleSelector> StyleButtons { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel? vm);
}