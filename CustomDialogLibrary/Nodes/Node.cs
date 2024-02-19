using System.Collections.ObjectModel;

namespace CustomDialogLibrary.Nodes;

/// <summary>
/// Node for sidebar tree
/// </summary>
/// <param name="title">Name of the node</param>
/// <param name="subNodes">Collection of children nodes</param>
public class Node(string title, ObservableCollection<INode>? subNodes = null) : INode
{
    /// <summary>
    /// Gets collection of children nodes
    /// </summary>
    public ObservableCollection<INode>? SubNodes { get; } = subNodes;
    public string Title { get; } = title;
    public bool Selectable => false;
}