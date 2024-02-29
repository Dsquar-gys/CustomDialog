using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.SideBarEntities;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class GeneralViewModel : ViewModelBase, IDisposable
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
    /// Gets <see cref="BodyStyleBox"/> for <see cref="BodyVM"/>
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
    
    public GeneralViewModel(ISpecificFileViewModel? sfvm = null)
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
            BodyVM!.ChangeFilterCommand.Execute(Filters[x]).Subscribe());

        // BodyStyleBox init
        BodyStyleSelectionBox = sfvm ?? new BodyStyleBox( 
        [
            new WrapPanelTemplate(),
            new DataGridTemplate()
        ]);
        
        // Body creation
        BodyVM = new BodyViewModel{ SpecificFileViewModel = BodyStyleSelectionBox };
        
        // Style of Body depends on BodyStyleBox.CurrentBodyTemplate
        BodyStyleSelectionBox.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(t => { BodyVM.CurrentStyle = t; });

        // Set filtering to All files
        BodyVM.ChangeFilterCommand.Execute(Filters.FirstOrDefault()!).Subscribe();
    }

    public void Dispose()
    {
        BodyVM.Dispose();
        FilterUpCommand.Dispose();
    }
}