using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.Interfaces;
using ReactiveUI;

namespace CustomDialogLibrary;

/// <summary>
/// Object that manages current and available body styles
/// </summary>
public class StyleBox : ReactiveObject, ISpecificFileViewModel
{
    private BodyTemplate? _selectedTemplate;
    private BodyTemplate? _currentBodyTemplate;

    public BodyTemplate? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    public ICommand? Command { get; }
    public ObservableCollection<BodyTemplate> StyleButtons { get; }

    /// <param name="buttonCollection">Collection of body styles</param>
    public StyleBox(IEnumerable<BodyTemplate> buttonCollection, ICommand? command = null)
    {
        Command = command;
        _selectedTemplate = buttonCollection.FirstOrDefault();
        StyleButtons = new(buttonCollection);
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
            _ => vm
        };
        return true;
    }
}