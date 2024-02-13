using System.Collections.ObjectModel;

namespace CustomDialogLibrary.Nodes;

public class Node : INode
{
    public ObservableCollection<INode>? SubNodes { get; }
    public string Title { get; }
    public bool Selectable => false;

    public Node(string title, ObservableCollection<INode> subNodes = null)
    {
        Title = title;
        SubNodes = subNodes;
    }
}