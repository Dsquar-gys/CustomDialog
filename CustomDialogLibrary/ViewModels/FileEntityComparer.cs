using CustomDialogLibrary.Models;

namespace CustomDialogLibrary.ViewModels;

public class FileEntityComparer : IComparer<FileEntityModelBase>
{
    public int Compare(FileEntityModelBase? x, FileEntityModelBase? y)
    {
        if( x is null || y is null )
        {
            return 0;
        }

        return x switch
        {
            DirectoryModel when y is FileModel => -1,
            FileModel when y is DirectoryModel => 1,
            FileModel dm when y is FileModel dm2 => string.Compare(dm.Name, dm2.Name,
                StringComparison.OrdinalIgnoreCase),
            DirectoryModel dm3 when y is DirectoryModel dm4 => string.Compare(dm3.Name, dm4.Name,
                StringComparison.OrdinalIgnoreCase),
            _ => 0
        };
    }
}