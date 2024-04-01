using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.History;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class OpenDialog : ReactiveObject
{
    private readonly BaseDialogWindow _mainWindow;
    private readonly WindowNotificationManager _notificationManager;
    private readonly ISpecificFileViewModel? _specificFileViewModel;
    private string? _directory;
    private bool _allowMultiple;

    public string? Directory
    {
        get => _directory;
        set => this.RaiseAndSetIfChanged(ref _directory, value);
    }

    public bool AllowMultiple
    {
        get => _allowMultiple;
        set => this.RaiseAndSetIfChanged(ref _allowMultiple, value);
    }
    
    public List<FileDialogFilter>? Filters { get; set; }
    
    public OpenDialog(ISpecificFileViewModel? specificFileViewModel = null)
    {
        _specificFileViewModel = specificFileViewModel ?? new BodyStyleBox( 
        [
            new WrapPanelTemplate(),
            new DataGridTemplate()
        ]);
        
        // Single selection
        _allowMultiple = false;
        this.WhenAnyValue(x => x.AllowMultiple)
            .Subscribe(b =>
            {
                foreach (var style in _specificFileViewModel.AvailableStyles)
                    style.AllowMultiple = b;
            });
        
        this.WhenAnyValue(x => x.Directory)
            .Subscribe(DirectoryHistory.ChangeDefaultDirectory);
        
        // Window init
        _mainWindow = new BaseDialogWindow();
        // Notification manager init
        _notificationManager = new(_mainWindow);
    }
    
    public Task<string[]?> ShowAsync(Window parent)
    {
        // Window View Model init
        var mainWindowViewModel = new BaseDialogWindowViewModel
        {
            DialogViewModel = new DialogViewModel(_specificFileViewModel) { ApplyTo = "Open" },
            OnLoaded = ReactiveCommand.Create<object?>(sender =>
            {
                if (sender is not BaseDialogWindow window) throw new ArgumentException();
        
                var content = window.GeneralControl.Content as DialogViewModel;

                content.WhenAnyValue(x => x.ToClose)
                    .Subscribe(toClose =>
                    {
                        if (!toClose) return;
                        
                        var multiple = content!.ContentVm.SelectedEntities;
                        string[] result;
                        
                        if (multiple.Count == 1)
                            result = [ multiple[0].FullPath ];
                        else
                            result = [..multiple.Select(x => x.FullPath)];
                        
                        
                    
                        window.Close(result.Length > 0 ? result.ToArray() : null);
                    });
            })
        };

        if (Filters is not null && Filters.Count > 0) mainWindowViewModel.DialogViewModel.Filters = Filters;
        
        // Command for dialog main view `Open` button
        mainWindowViewModel.DialogViewModel.InvokeDialogAssignment = ReactiveCommand.CreateFromTask(() =>
        {
            var body = mainWindowViewModel.DialogViewModel.ContentVm;
            
            if (body.SelectedEntities.Count > 1)
            {
                if (!body.SelectedEntities.Select(x => x.GetType()).Contains(typeof(DirectoryModel)))
                    body.FilePath = body.SelectedEntities.FirstOrDefault()!.FullPath;
                else
                    _notificationManager.Show(new Notification("Folders found",
                        "Multiple selection found at least 1 folder. Folders can only be opened alone.",
                        NotificationType.Error));
            }
            else if (body.SelectedEntities.Count == 1)
                body.FilePath = body.SelectedEntities.FirstOrDefault()!.FullPath;
            else
                _notificationManager.Show(new Notification("Null selection",
                "Choose entity to open.",
                NotificationType.Warning));
            return Task.CompletedTask;
        });

        _mainWindow.DataContext = mainWindowViewModel;
        
        return _mainWindow.ShowDialog<string[]?>(parent);
    }
}