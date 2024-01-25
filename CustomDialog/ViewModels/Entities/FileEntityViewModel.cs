using CustomDialog.Models;

namespace CustomDialog.ViewModels.Entities;

public abstract class FileEntityViewModel : ViewModelBase, ILoadable
{
    public string Title { get; }
    public string FullPath { get; set; }
    protected FileEntityViewModel(string title) => Title = title;
}