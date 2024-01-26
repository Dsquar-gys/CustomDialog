using CustomDialog.Models;
using CustomDialog.ViewModels.Entities;

namespace CustomDialog.ViewModels.History;

public class DirectoryNode
{
    public DirectoryNode? PreviousNode { get; set; }
    public DirectoryNode? NextNode { get; set; }
    
    public string DirectoryPath { get; }
    public string DirectoryPathName { get; }

    public DirectoryNode(string directoryPath, string directoryPathName)
    {
        DirectoryPath = directoryPath;
        DirectoryPathName = directoryPathName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is DirectoryNode node)
            return DirectoryPath == node.DirectoryPath &&
                   DirectoryPathName == node.DirectoryPathName;

        return false;
    }
}