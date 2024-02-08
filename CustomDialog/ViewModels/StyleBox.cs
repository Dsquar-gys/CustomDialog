using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using CustomDialog.Models;
using CustomDialog.Models.Entities;
using CustomDialog.Models.Interfaces;
using CustomDialog.ViewModels.Commands;
using CustomDialog.Views.BodyTemplates;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class StyleBox : ReactiveObject, ISpecificFileViewModel
{
    private StyleSelector _selectedStyle;
    private BodyTemplate _currentBodyTemplate;

    public StyleSelector SelectedStyle
    {
        get => _selectedStyle;
        set => this.RaiseAndSetIfChanged(ref _selectedStyle, value);
    }
    public BodyTemplate? CurrentBodyTemplate
    {
        get => _currentBodyTemplate;
        set => this.RaiseAndSetIfChanged(ref _currentBodyTemplate, value);
    }
    public ICommand Command => new DelegateCommand(x =>
    { });
    public ObservableCollection<StyleSelector> StyleButtons { get; }

    public StyleBox(IEnumerable<StyleSelector> buttonCollection)
    {
        _selectedStyle = buttonCollection.FirstOrDefault();
        _currentBodyTemplate = SelectedStyle.StyleTemplate;
        StyleButtons = new(buttonCollection);
        this.WhenAnyValue(x => x.SelectedStyle)
            .Subscribe(_ => ChangeCurrentTemplate());
    }
    
    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm)
    {
        vm = null!;
        
        if (CurrentBodyTemplate is EmptyTemplate)
            return false;

        if (file is FileInfo fileInfo)
            vm = new FileModel(fileInfo);
        
        if(file is DirectoryInfo directoryInfo)
            vm = new DirectoryModel(directoryInfo);
        return true;
    }

    private void ChangeCurrentTemplate() => CurrentBodyTemplate = SelectedStyle.StyleTemplate;
}