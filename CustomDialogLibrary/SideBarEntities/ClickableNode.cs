namespace CustomDialogLibrary.SideBarEntities;

/// <summary>
/// Selectable node for sidebar tree
/// </summary>
/// <param name="path">Path of directory the node should lead</param>
/// <param name="title">Name of the node</param>
public class ClickableNode(string path, string title)
{
    /// <summary>
    /// Leads to some directory
    /// </summary>
    public string Path { get; } = path;
    public string Title { get; } = title;
}