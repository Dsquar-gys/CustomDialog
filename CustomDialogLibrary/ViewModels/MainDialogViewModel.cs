using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.SideBarEntities;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class MainDialogViewModel : ViewModelBase, IDisposable
{
    #region Private Fields
    
    private ClickableNode? _selectedNode;
    private bool _toClose;

    #endregion
    
    #region Properties

    /// <summary>
    /// Body for content
    /// </summary>
    public ContentBodyViewModel ContentBodyVm { get; }

    /// <summary>
    /// Gets <see cref="BodyStyleBox"/> for <see cref="ContentBodyVm"/>
    /// </summary>
    public ISpecificFileViewModel BodyStyleSelectionBox { get; }
    
    /// <summary>
    /// Gets collection of sidebar tree nodes
    /// </summary>
    public ObservableCollection<SideBarNode> SideBarNodes { get; }
    
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
    public List<FileDialogFilter> Filters { get; set; } =
    [
        new FileDialogFilter { Name = "All Files", Extensions = ["*"] },
        new FileDialogFilter { Name = "Images", Extensions = MediaFormats.ImageExtensions.ToList() },
        new FileDialogFilter { Name = "Audio", Extensions = MediaFormats.AudioExtensions.ToList() },
        new FileDialogFilter { Name = "Videos", Extensions = MediaFormats.VideoExtensions.ToList() }
    ];

    public bool ToClose
    {
        get => _toClose;
        private set => this.RaiseAndSetIfChanged(ref _toClose, value);
    }
    
    public required string ApplyTo { get; init; }
    
    #endregion

    #region Commands

    /// <summary>
    /// Reactive command for filtering content
    /// </summary>
    public ReactiveCommand<int, Unit> FilterUpCommand { get; }
    
    public ReactiveCommand<Unit, Unit> InvokeDialogAssignment { get; set; }
    
    #endregion
    
    public MainDialogViewModel(ISpecificFileViewModel? sfvm = null)
    {
        // Sidebar tree nodes init
        SideBarNodes = new ObservableCollection<SideBarNode>
        {
            new ("System", [
                new ClickableNode("/", "Root")
            ]),
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
            ContentBodyVm!.ChangeFilterCommand.Execute(Filters[x]).Subscribe());

        // BodyStyleBox init
        BodyStyleSelectionBox = sfvm ?? new BodyStyleBox( 
        [
            new WrapPanelTemplate(),
            new DataGridTemplate()
        ]);
        
        // Body creation
        ContentBodyVm = new ContentBodyViewModel{ SpecificFileViewModel = BodyStyleSelectionBox };
        
        // Style of Body depends on BodyStyleBox.CurrentBodyTemplate
        BodyStyleSelectionBox.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(t => { ContentBodyVm.CurrentStyle = t; });

        // Set filtering to All files
        ContentBodyVm.ChangeFilterCommand.Execute(Filters.FirstOrDefault()!).Subscribe();

        ContentBodyVm.WhenAnyValue(x => x.ToClose)
            .Subscribe(x => ToClose = x);
    }

    public void Dispose()
    {
        ContentBodyVm.Dispose();
        FilterUpCommand.Dispose();
    }
}