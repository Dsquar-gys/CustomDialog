using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CustomDialog.Models;
using CustomDialog.Models.Nodes;
using CustomDialog.Views.BodyTemplates;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    #region Private Fields
    
    private ClickableNode _selectedNode;
    
    #endregion
    
    #region Properties

    public BodyViewModel BodyVM { get; }
    public StyleBox StyleSelectorBox => new( 
        [
            new StyleSelector(new WrapPanelTemplate(), "plates"),
            new StyleSelector(new DataGridTemplate(), "grid")
        ]);

    public ObservableCollection<Node> Nodes { get; }

    public ClickableNode SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }
    
    #endregion

    public GeneralViewModel()
    {
        // Nodes initialization
        Nodes = new ObservableCollection<Node>
        {
            new Node("Places", [
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Home"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop"),
                new ClickableNode(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),"Download"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents"),
                new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures")
            ])
        };
        
        BodyVM = new BodyViewModel
        {
            SelectedStyle = new EmptyTemplate()
        };
    }
}