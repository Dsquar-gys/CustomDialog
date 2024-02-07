using System;
using System.Collections.ObjectModel;
using System.IO;
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
    public ObservableCollection<Node> Nodes{ get; }

    public ObservableCollection<SpecificFileViewModel> TemplateStyles =>
    [
        new SpecificFileViewModel(new WrapPanelTemplate(), null, "plates"),
        new SpecificFileViewModel(new DataGridTemplate(), null, "grid")
    ];

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

        BodyVM = new()
        {
            SelectedStyle = TemplateStyles[0]
        };
    }
}