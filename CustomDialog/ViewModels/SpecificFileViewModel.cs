using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.Windows.Input;
using Avalonia.Controls;
using CustomDialog.Models;
using CustomDialog.Models.Entities;
using CustomDialog.Models.Interfaces;
using CustomDialog.ViewModels.Commands;
using CustomDialog.Views.BodyTemplates;

namespace CustomDialog.ViewModels;

public class StyleBox(IEnumerable<StyleSelector> buttonCollection) : ISpecificFileViewModel
{
    public BodyTemplate? CurrentBodyTemplate { get; private set; } = new WrapPanelTemplate();
    public ICommand? Command => new DelegateCommand(x =>
    {
        if (x is ListBox listBox)
            CurrentBodyTemplate = (listBox.SelectedItem as StyleSelector)!.StyleTemplate;
    });
    public ObservableCollection<StyleSelector> StyleButtons { get; } = new(buttonCollection);
    
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
}