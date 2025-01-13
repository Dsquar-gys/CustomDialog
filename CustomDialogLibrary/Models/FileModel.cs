namespace CustomDialogLibrary.Models;

public sealed class FileModel : FileEntityModelBase
{
    public FileModel(FileInfo file) : base(file) 
    {
        IconName = string.Empty;
    }
}