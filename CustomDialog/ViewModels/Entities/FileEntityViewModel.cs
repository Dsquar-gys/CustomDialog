namespace CustomDialog.ViewModels.Entities;

public abstract class FileEntityViewModel : ViewModelBase
{
    public string Name { get; }
    public string FullName { get; set; }
    protected FileEntityViewModel(string name) => Name = name;
}