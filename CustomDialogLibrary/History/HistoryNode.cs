namespace CustomDialogLibrary.History;

/// <summary>
/// SideBarNode (page) for history
/// </summary>
/// <param name="path">Full path of the entity</param>
public class HistoryNode(string path)
{
    public HistoryNode? PreviousNode { get; set; }
    public HistoryNode? NextNode { get; set; }
    
    /// <summary>
    /// Gets full path of the directory
    /// </summary>
    public string Path { get; } = path;

    // To compare nodes
    public override bool Equals(object? obj)
    {
        if (obj is HistoryNode node)
            return Path == node.Path;

        return false;
    }
    
    public override int GetHashCode() => throw new NotImplementedException();

    public static bool IsNull(HistoryNode? node) => node == null;
}