using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Nodes;
using ReactiveUI;

namespace DemoApp.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    #region Private Fields
    
    private ClickableNode? _selectedNode;

    #endregion
    
    #region Properties

    public BodyViewModel BodyVM { get; }
    public StyleBox StyleSelectorBox { get; } = new( 
    [
        new StyleSelector(new WrapPanelTemplate(), "plates"),
        new StyleSelector(new DataGridTemplate(), "grid")
    ]);

    public ObservableCollection<Node> Nodes { get; }
    public ClickableNode? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    public List<FileDialogFilter> Filters => new()
    {
        new FileDialogFilter { Name = "All Files", Extensions = [""] },
        new FileDialogFilter { Name = "Images", Extensions = [".png", ".svg", ".jpg"]},
        new FileDialogFilter { Name = ".png", Extensions = [".png"] },
        new FileDialogFilter { Name = ".svg", Extensions = [".svg"] },
    };
    
    #endregion

    #region Commands

    public ReactiveCommand<int, Unit> FilterUpCommand { get; }
    
    #endregion
    
    public GeneralViewModel()
    {
        Nodes = new ObservableCollection<Node>
        {
            new("Places", [
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Home"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop"),
                new ClickableNode(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),"Download"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures")
            ])
        };

        FilterUpCommand = ReactiveCommand.Create<int>(x => 
            BodyVM!.ChangeFilterReactiveCommand.Execute(Filters[x]).Subscribe());

        BodyVM = new BodyViewModel();
        StyleSelectorBox.WhenAnyValue(x => x.CurrentBodyTemplate)
            .Subscribe(t => { BodyVM.CurrentStyle = t; });

        BodyVM.ChangeFilterReactiveCommand.Execute(Filters.FirstOrDefault()).Subscribe();
    }
}