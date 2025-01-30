using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Models;

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