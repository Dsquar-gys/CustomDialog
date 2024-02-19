namespace CustomDialogLibrary.History;

/// <summary>
/// Node (page) for directories openings history
/// </summary>
/// <param name="directoryPath">Full path of the directory</param>
/// <param name="directoryPathName">Title of the directory</param>
public class DirectoryNode(string directoryPath, string directoryPathName)
{
    public DirectoryNode? PreviousNode { get; set; }
    public DirectoryNode? NextNode { get; set; }
    
    /// <summary>
    /// Gets full path of the directory
    /// </summary>
    public string DirectoryPath { get; } = directoryPath;
    
    /// <summary>
    /// Gets title of the directory
    /// </summary>
    public string DirectoryPathName { get; } = directoryPathName;

    // To compare nodes
    public override bool Equals(object? obj)
    {
        if (obj is DirectoryNode node)
            return DirectoryPath == node.DirectoryPath &&
                   DirectoryPathName == node.DirectoryPathName;

        return false;
    }
    
    public override int GetHashCode() => throw new NotImplementedException();

    public static bool IsNull(DirectoryNode? node) => node == null;
}