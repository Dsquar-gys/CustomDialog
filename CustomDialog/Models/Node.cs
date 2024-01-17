using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace CustomDialog.Models;

public class Node : INode
{
    public ObservableCollection<INode>? SubNodes { get; }
    public string Title { get; }
    public bool Selectable { get; } = false;

    public Node(string title) => Title = title;
    public Node(string title, ObservableCollection<INode> subNodes)
    {
        Title = title;
        SubNodes = subNodes;
    }
}