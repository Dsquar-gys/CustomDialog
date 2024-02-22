using System.Collections.ObjectModel;

namespace CustomDialogLibrary.SideBarEntities;

/// <summary>
/// SideBarNode for sidebar tree
/// </summary>
/// <param name="title">Name of the node</param>
/// <param name="subNodes">Collection of children nodes</param>
public class SideBarNode(string title, ObservableCollection<ClickableNode>? subNodes = null)
{
    /// <summary>
    /// Gets collection of children nodes
    /// </summary>
    public ObservableCollection<ClickableNode>? SubNodes { get; } = subNodes;
    public string Title { get; } = title;
}