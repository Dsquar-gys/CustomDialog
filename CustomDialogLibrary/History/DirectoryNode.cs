namespace CustomDialogLibrary.History;

public class DirectoryNode(string directoryPath, string directoryPathName)
{
    public DirectoryNode? PreviousNode { get; set; }
    public DirectoryNode? NextNode { get; set; }
    
    public string DirectoryPath { get; } = directoryPath;
    public string DirectoryPathName { get; } = directoryPathName;

    public override bool Equals(object? obj)
    {
        if (obj is DirectoryNode node)
            return DirectoryPath == node.DirectoryPath &&
                   DirectoryPathName == node.DirectoryPathName;

        return false;
    }
}