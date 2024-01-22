using System.Collections.ObjectModel;
using CustomDialog.Models.Nodes;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    #region TreeView
    
    public ObservableCollection<Node> Nodes{ get; }
    
    #endregion

    public GeneralViewModel()
    {
        // Nodes initialization
        Nodes = new ObservableCollection<Node>
        {
            new Node("Places", [
                new ClickableNode("Home"),
                new ClickableNode("Desktop"),
                new ClickableNode("Download")
            ]),
            new Node("FloPPaSS", [
                new ClickableNode("FloPPa1"),
                new ClickableNode("FloPPa2")
            ])
        };
    }
}