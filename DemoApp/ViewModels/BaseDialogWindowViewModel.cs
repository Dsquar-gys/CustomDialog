using CustomDialogLibrary.ViewModels;

namespace DemoApp.ViewModels;

public class BaseDialogWindowViewModel : ViewModelBase
{
    public required FileDialogVM FileDialogVM { get; init; }
}