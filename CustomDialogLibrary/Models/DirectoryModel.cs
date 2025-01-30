namespace CustomDialogLibrary.Models;

public sealed class DirectoryModel : FileEntityModelBase
{
    public DirectoryModel(DirectoryInfo directory) : base(directory) 
    {
        IconName = "FolderModelIcon";
    }
}