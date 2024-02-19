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
    private StyleSelector? _selectedStyle;
    private BodyTemplate? _currentBodyTemplate;

    public StyleSelector? SelectedStyle
    {
        get => _selectedStyle;
        set => this.RaiseAndSetIfChanged(ref _selectedStyle, value);
    }
    public BodyTemplate? CurrentBodyTemplate
    {
        get => _currentBodyTemplate;
        private set => this.RaiseAndSetIfChanged(ref _currentBodyTemplate, value);
    }
    public ICommand? Command { get; }
    public ObservableCollection<StyleSelector> StyleButtons { get; }

    /// <param name="buttonCollection">Collection of body styles</param>
    public StyleBox(IEnumerable<StyleSelector> buttonCollection, ICommand? command = null)
    {
        Command = command;
        _selectedStyle = buttonCollection.FirstOrDefault();
        _currentBodyTemplate = SelectedStyle!.StyleTemplate;
        StyleButtons = new(buttonCollection);
        
        // Subscription ChangeCurrentTemplate on SelectedStyle change
        this.WhenAnyValue(x => x.SelectedStyle)
            .Subscribe(_ => ChangeCurrentTemplate());
    }
    
    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel? vm)
    {
        vm = null;
        if (CurrentBodyTemplate is EmptyTemplate)
            return false;

        vm = file switch
        {
            FileInfo fileInfo => new FileModel(fileInfo),
            DirectoryInfo directoryInfo => new DirectoryModel(directoryInfo),
            _ => vm
        };
        return true;
    }

    private void ChangeCurrentTemplate() => CurrentBodyTemplate = SelectedStyle!.StyleTemplate;
}