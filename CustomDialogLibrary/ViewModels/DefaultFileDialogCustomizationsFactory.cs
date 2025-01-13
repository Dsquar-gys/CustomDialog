using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.Models;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

/// <summary>
/// Object that manages current and available body styles
/// </summary>
public class DefaultFileDialogCustomizationsFactory : ReactiveObject, IDialogCustomizationsFactory
{
    private FolderListingTemplate? _selectedTemplate;

    public FolderListingTemplate? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    public FolderListingTemplate[] AvailableStyles { get; }
    
    /// <param name="buttonCollection">Collection of body styles</param>
    public DefaultFileDialogCustomizationsFactory(IEnumerable<FolderListingTemplate> buttonCollection)
    {
        _selectedTemplate = buttonCollection.FirstOrDefault();
        
        AvailableStyles = buttonCollection.ToArray();
    }
    
    public Task<FileEntityModelBase?> TryToCreateFileEntry(FileSystemInfo? file)
    {
        if( file is null )
        {
            return Task.FromResult<FileEntityModelBase?>( null );
        }

        if( SelectedTemplate is EmptyTemplate )
        {
            return Task.FromResult<FileEntityModelBase?>( null );
        }

        FileEntityModelBase? vm;

        switch( file )
        {
            case FileInfo fileInfo:
                vm = new FileModel( fileInfo );
                break;
            case DirectoryInfo directoryInfo:
                vm = new DirectoryModel( directoryInfo );
                break;
            default:
                vm = null;

                break;
        }

        return Task.FromResult( vm );
    }
}