using System;
using System.Windows.Input;

namespace CustomDialog.ViewModels.Commands;

public class DelegateCommand(Action<object> execute, Predicate<object>? canExecute = null)
    : ICommand
{
    public bool CanExecute(object? parameter) => canExecute == null || canExecute.Invoke(parameter!);
    public void Execute(object? parameter) => execute?.Invoke(parameter!);
    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}