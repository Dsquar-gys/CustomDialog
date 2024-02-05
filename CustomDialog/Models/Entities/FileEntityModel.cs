using CustomDialog.ViewModels;

namespace CustomDialog.Models.Entities;

public abstract class FileEntityModel : ViewModelBase
{
    public string Title { get; set; }
    public string FullPath { get; }
    public ISpecificFileViewModel Svm { get; }

    protected FileEntityModel(ISpecificFileViewModel vm, string path, string? title = null)
    {
        FullPath = path;
        Title = title ?? FullPath;
        Svm = vm;

        string type = "";
    }
}