using System.Reactive;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class BaseDialogWindowViewModel : ViewModelBase
{
    public required DialogViewModel DialogViewModel { get; init; }
    public required ReactiveCommand<object?, Unit> OnLoaded { get; init; }
}