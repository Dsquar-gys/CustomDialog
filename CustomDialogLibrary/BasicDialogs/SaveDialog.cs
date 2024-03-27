using Avalonia.Controls;
using CustomDialogLibrary.History;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class SaveDialog : ReactiveObject
{
    private readonly BaseDialogWindow _mainWindow;
    private string? _directory;

    public string? Directory
    {
        get => _directory;
        set => this.RaiseAndSetIfChanged(ref _directory, value);
    }

    public string? InitialFileName { get; set; }

    public string? DefaultExtension { get; set; }

    public SaveDialog()
    {
        this.WhenAnyValue(x => x.Directory)
            .Subscribe(DirectoryHistory.ChangeDefaultDirectory);
        
        // Window init
        _mainWindow = new BaseDialogWindow();
    }

    public Task<string?> ShowAsync(Window parent)
    {
        // Window View Model init
        var mainWindowViewModel = new BaseDialogWindowViewModel
        {
            DialogViewModel = new DialogViewModel { ApplyTo = "Save" },
            OnLoaded = ReactiveCommand.Create<object?>(sender =>
            {
                if (sender is not BaseDialogWindow window) throw new ArgumentException();
        
                var content = window.GeneralControl.Content as DialogViewModel;

                content.WhenAnyValue(x => x.ToClose)
                    .Subscribe(toClose =>
                    {
                        if (!toClose) return;
                        
                        var multiple = content!.ContentVm.SelectedEntities;

                        var result = multiple.Count == 1
                            ? multiple[0].FullPath
                            : Path.Combine(content.ContentVm.FilePath!, InitialFileName + "." + DefaultExtension);
                    
                        window.Close(result);
                    });
            })
        };
        
        // Command for dialog main view `Save` button
        mainWindowViewModel.DialogViewModel.InvokeDialogAssignment = ReactiveCommand.Create(() =>
        {
            var body = mainWindowViewModel.DialogViewModel.ContentVm;
            var multiple = body.SelectedEntities;
            
            // if selected --> overwrite
            if (multiple.Count == 1)
                body.FilePath = multiple[0].FullPath;
            else // if not --> create
                _mainWindow.Close(Path.Combine(body.FilePath!, InitialFileName + "." + DefaultExtension));
        });
        
        _mainWindow.DataContext = mainWindowViewModel;
        
        return _mainWindow.ShowDialog<string?>(parent);
    }
}