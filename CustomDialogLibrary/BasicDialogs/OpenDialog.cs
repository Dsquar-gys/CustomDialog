using System.Reactive.Subjects;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.History;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class OpenDialog : ReactiveObject
{
    private readonly Subject<(string FileName, object? Tag, object Mode)> _pending;
    private readonly Window _mainWindow;
    private readonly WindowNotificationManager _notificationManager;
    private readonly IDialogCustomizationsFactory? _specificFileViewModel;
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
    
    public IObservable<(string FileName, object? Tag, object Mode)> Pending { get; }
    
    public OpenDialog(Window parent)
    {
        _pending = new Subject<(string FileName, object? Tag, object Mode)>();
        Pending = _pending;
        
        // _specificFileViewModel = specificFileViewModel ?? new BodyStyleBox( 
        // [
        //     new WrapPanelTemplate(),
        //     new DataGridTemplate()
        // ]);
        //
        // // Single selection
        // _allowMultiple = false;
        // this.WhenAnyValue(x => x.AllowMultiple)
        //     .Subscribe(b =>
        //     {
        //         foreach (var style in _specificFileViewModel.AvailableStyles)
        //             style.AllowMultiple = b;
        //     });
        //
        // this.WhenAnyValue(x => x.Directory)
        //     .Subscribe(DirectoryHistory.ChangeDefaultDirectory);
        
        // Window init
        _mainWindow = parent;
        // Notification manager init
        _notificationManager = new(_mainWindow);
    }
    
    public async Task AskUser( object mode )
    {
        Guard.IsNotNull( _mainWindow );
        
        var fileNames = await ShowDialogAsync();

        foreach( var fileName in fileNames )
        {
            var ext = Path.GetExtension( fileName )
                .TrimStart( '.' );

            // var tag = Array.Find( settings.Filters, filter => filter.Extensions.Contains( ext ) )
            //     ?.Tag;

            _pending.OnNext( ( fileName, null, mode ) );

            // DefaultSettings = DefaultSettings with
            // {
            //     InitialDirectory = Path.GetDirectoryName( fileName ) ??
            //                        Environment.GetFolderPath( Environment.SpecialFolder.Personal )
            // };
        }
    }
    
    public Task<string[]> ShowDialogAsync()
    {
        Guard.IsNotNull( _mainWindow );

        string initialDirectory = string.Empty;
    
        if( string.IsNullOrWhiteSpace( initialDirectory ) )
        {
            initialDirectory =  Environment.GetFolderPath(  Environment.SpecialFolder.MyDocuments );
        }
    
        var vm = new FileDialogVM(
            initialDirectory,
            new DefaultFileDialogCustomizationsFactory(
            [
                new WrapPanelTemplate(),
                new DataGridTemplate()
            ] ) );
      
        vm.Filters = Filters
            .Select( filter => new FilePickerFileType( filter.Name )
            {
                Patterns = filter.Extensions.Select( p => $"*.{p}" ).ToArray()
            } )
            .ToArray();

        var selection = vm.FileList.SelectedEntities ?? Array.Empty<FileEntityModelBase>();

        return _mainWindow.ShowDialog<string[]>(_mainWindow);
    }
}