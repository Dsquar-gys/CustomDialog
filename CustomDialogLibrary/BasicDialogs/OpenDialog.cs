using Avalonia.Controls;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class OpenDialog
{
    private BaseDialogWindowViewModel _mainWindowViewModel;
    private readonly BaseDialogWindow _mainWindow;
    
    public string? InitialFileName { get; set; }
    public string? Directory { get; set; }
    public string? Title { get; set; }

    public OpenDialog(ISpecificFileViewModel? specificFileViewModel = null)
    {
        // Window View Model init
        _mainWindowViewModel = new BaseDialogWindowViewModel
        {
            MainDialogViewModel = new MainDialogViewModel(specificFileViewModel) { ApplyTo = "Open" },
            OnLoaded = ReactiveCommand.Create<object?>(sender =>
            {
                if (sender is not BaseDialogWindow window) throw new ArgumentException();
        
                var content = window.GeneralControl.Content as MainDialogViewModel;

                content.WhenAnyValue(x => x.ToClose)
                    .Subscribe(toClose =>
                    {
                        if (!toClose) return;
                        var entity = content!.ContentBodyVm.SelectedFileEntity;
                        List<string> result = new();
                
                        if (entity is FileModel)
                            result.Add(entity.FullPath);
                    
                        window.Close(result.Count > 0 ? result.ToArray() : null);
                    });
            })
        };
        
        _mainWindowViewModel.MainDialogViewModel.InvokeDialogAssignment = ReactiveCommand.CreateFromTask(() =>
        {
            _mainWindowViewModel.MainDialogViewModel.ContentBodyVm.Open();
            return Task.CompletedTask;
        }, _mainWindowViewModel.MainDialogViewModel.ContentBodyVm.WhenAnyValue(x => x.SelectedFileEntity,
            selector: entity => entity is FileModel));

        // Window init
        _mainWindow = new BaseDialogWindow
        {
            DataContext = _mainWindowViewModel
        };
    }
    
    public Task<string[]?> ShowAsync(Window parent) => _mainWindow.ShowDialog<string[]?>(parent);
}