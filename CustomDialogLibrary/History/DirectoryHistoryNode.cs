namespace CustomDialogLibrary.History;

/// <summary>
/// SideBarNode (page) for history
/// </summary>
/// <param name="path">Full path of the entity</param>
public class DirectoryHistoryNode(string path)
{
    public DirectoryHistoryNode? PreviousNode { get; set; }
    public DirectoryHistoryNode? NextNode { get; set; }
    
    /// <summary>
    /// Gets full path of the directory
    /// </summary>
    public string Path { get; } = path;

    // To compare nodes
    public override bool Equals(object? obj)
    {
        if (obj is DirectoryHistoryNode node)
            return Path == node.Path;

        return false;
    }

    public override int GetHashCode() => Path.GetHashCode();
}