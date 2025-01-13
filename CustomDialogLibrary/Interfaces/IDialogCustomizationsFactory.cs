using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Models;
using ReactiveUI;

namespace CustomDialogLibrary.Interfaces;

public interface IDialogCustomizationsFactory
{
    /// <summary>
    /// <see cref="FolderListingTemplate"/> that is selected currently
    /// </summary>
    FolderListingTemplate? SelectedTemplate { get; set; }
    
    /// <summary>
    /// Collection of available body styles
    /// </summary>
    FolderListingTemplate[] AvailableStyles { get; }
    
    Task<FileEntityModelBase?> TryToCreateFileEntry(FileSystemInfo? file);
}