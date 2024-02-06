using CustomDialog.Models;

namespace CustomDialog.ViewModels.Entities;

public abstract class FileEntityModel : ViewModelBase, IImagable
{
    public string Title { get; set; }
    public string FullPath { get; set; }
    public string IconName { get; }

    protected FileEntityModel(string path, string? title = null, string? iconName = null)
    {
        Title = title ?? path;
        FullPath = path;
        IconName = iconName;

        string type = "";
    }
}