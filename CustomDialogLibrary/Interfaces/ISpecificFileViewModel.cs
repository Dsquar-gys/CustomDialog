using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using ReactiveUI;

namespace CustomDialogLibrary.Interfaces;

public interface ISpecificFileViewModel
{
    /// <summary>
    /// <see cref="BodyTemplate"/> that is selected currently
    /// </summary>
    BodyTemplate? SelectedTemplate { get; }
    
    ICommand? Command { get; }
    
    /// <summary>
    /// Collection of available body styles
    /// </summary>
    ObservableCollection<BodyTemplate> AvailableStyles { get; }
    bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel? vm);
}