using System.Collections.ObjectModel;
using CustomDialog.Models;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    #region TreeView
    
    public ObservableCollection<Node> Nodes{ get; }
    
    #endregion

    public GeneralViewModel()
    {
        Nodes = new ObservableCollection<Node>
        {
            new Node("Places", new ObservableCollection<INode>
            {
                new ClickableNode("Home"),
                new ClickableNode("Desktop"),
                new ClickableNode("Download")
            }),
            new Node("FloPPaSS", new ObservableCollection<INode>
            {
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
                new ClickableNode("FloPPa"),
            })
        };
    }
}