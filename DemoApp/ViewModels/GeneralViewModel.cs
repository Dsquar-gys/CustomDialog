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

    /// <summary>
    /// Body for content
    /// </summary>
    public BodyViewModel BodyVM { get; }
    
    /// <summary>
    /// Gets <see cref="StyleBox"/> for <see cref="BodyVM"/>
    /// </summary>
    public StyleBox StyleSelectorBox { get; }
    
    /// <summary>
    /// Gets collection of sidebar tree nodes
    /// </summary>
    public ObservableCollection<Node> Nodes { get; }
    
    /// <summary>
    /// Gets Selected node on sidebar tree
    /// </summary>
    public ClickableNode? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    /// <summary>
    /// Gets collection of filters for content by extensions
    /// </summary>
    public List<FileDialogFilter> Filters =>
    [
        new FileDialogFilter { Name = "All Files", Extensions = [""] },
        new FileDialogFilter { Name = "Images", Extensions = [".png", ".svg", ".jpg"] },
        new FileDialogFilter { Name = ".png", Extensions = [".png"] },
        new FileDialogFilter { Name = ".svg", Extensions = [".svg"] }
    ];
    
    #endregion

    #region Commands

    /// <summary>
    /// Reactive command for filtering content
    /// </summary>
    public ReactiveCommand<int, Unit> FilterUpCommand { get; }
    
    #endregion
    
    public GeneralViewModel()
    {
        // Sidebar tree nodes init
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

        // Filtering command creation
        FilterUpCommand = ReactiveCommand.Create<int>(x => 
            BodyVM!.ChangeFilterReactiveCommand.Execute(Filters[x]).Subscribe());

        // Body creation
        BodyVM = new BodyViewModel();
        
        // StyleBox init
        StyleSelectorBox  = new( 
        [
            new StyleSelector(new WrapPanelTemplate(), "plates"),
            new StyleSelector(new DataGridTemplate(), "grid")
        ]);
        // Style of Body depends on StyleBox.CurrentBodyTemplate
        StyleSelectorBox.WhenAnyValue(x => x.CurrentBodyTemplate)
            .Subscribe(t => { BodyVM.CurrentStyle = t; });

        // Set filtering to All files
        BodyVM.ChangeFilterReactiveCommand.Execute(Filters.FirstOrDefault()).Subscribe();
    }
}