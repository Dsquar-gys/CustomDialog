using System.Reactive;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class BaseDialogWindowViewModel : ViewModelBase
{
    public required MainDialogViewModel MainDialogViewModel { get; init; }
    public required ReactiveCommand<object?, Unit> OnLoaded { get; init; }
}