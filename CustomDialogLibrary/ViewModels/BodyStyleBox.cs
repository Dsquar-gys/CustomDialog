using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.Interfaces;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

/// <summary>
/// Object that manages current and available body styles
/// </summary>
public class BodyStyleBox : ReactiveObject, ISpecificFileViewModel
{
    private BodyTemplate? _selectedTemplate;

    public BodyTemplate? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    public ICommand? Command { get; }
    public ObservableCollection<BodyTemplate> AvailableStyles { get; }

    /// <param name="buttonCollection">Collection of body styles</param>
    public BodyStyleBox(IEnumerable<BodyTemplate> buttonCollection, ICommand? command = null)
    {
        Command = command;
        _selectedTemplate = buttonCollection.FirstOrDefault();
        AvailableStyles = new(buttonCollection);
    }
    
    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel? vm)
    {
        vm = null;
        if (SelectedTemplate is EmptyTemplate)
            return false;

        vm = file switch
        {
            FileInfo fileInfo => new FileModel(fileInfo),
            DirectoryInfo directoryInfo => new DirectoryModel(directoryInfo),
            _ => throw new ArgumentException("Wrong type FileEntityModel... (TryToCreateFileEntry)")
        };
        return true;
    }
}