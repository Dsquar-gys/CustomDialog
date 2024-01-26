using System.Collections.ObjectModel;
using CustomDialog.Models.Nodes;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    #region Private Fields
    
    private ClickableNode _selectedNode;
    
    #endregion
    
    #region Properties
    
    public BodyViewModel BodyVM { get; } = new();
    public ObservableCollection<Node> Nodes{ get; }

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
                new ClickableNode("/home/dmitrichenkoda@kvant-open.spb.ru", "Home"),
                new ClickableNode("/home/dmitrichenkoda@kvant-open.spb.ru/Desktop", "Desktop"),
                new ClickableNode("/home/dmitrichenkoda@kvant-open.spb.ru/Downloads","Download")
            ]),
            new Node("FloPPaSS", [
                new ClickableNode("FloPPa1", "FloPPa1"),
                new ClickableNode("FloPPa2", "FloPPa2")
            ])
        };
    }
}