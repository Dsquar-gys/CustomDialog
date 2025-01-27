using System.Reactive.Subjects;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class OpenDialog : ReactiveObject
{
    private readonly Window? _mainWindow;
    private readonly Subject<string> _pending;
    
    public OpenDialog()
    {
        DefaultSettings = new OpenFileDialogOptions
        {
            AllowMultiple    = false,
            Caption          = "Open File",
            InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal )
        };

        _pending = new();
        Pending = _pending;
        
        // Window init
        _mainWindow = new BaseDialogWindow();
    }
    
    public OpenFileDialogOptions DefaultSettings { get; set; }
    
    public IObservable<string> Pending { get; }
    
    public async Task AskUser( Window parent,
                               OpenFileDialogOptions? settingsOverride = null)
    {
        Guard.IsNotNull( _mainWindow );
        
        var options = settingsOverride ?? DefaultSettings;
        
        var fileNames = await ShowDialogAsync(options, parent);

        foreach( var fileName in fileNames )
        {
            var ext = Path.GetExtension( fileName )
                .TrimStart( '.' );

            // var tag = Array.Find( settings.Filters, filter => filter.Extensions.Contains( ext ) )
            //     ?.Tag;
            //
            // _pending.OnNext( ( fileName, tag, mode ) );
            //
            // DefaultSettings = DefaultSettings with
            // {
            //     InitialDirectory = Path.GetDirectoryName( fileName ) ??
            //                        Environment.GetFolderPath( Environment.SpecialFolder.Personal )
            // };
        }
    }
    
    public Task<string[]> ShowDialogAsync( OpenFileDialogOptions options, Window parent )
    {
        Guard.IsNotNull( _mainWindow );

        var initialDirectory = string.Empty;
        
        if( !string.IsNullOrWhiteSpace( options.InitialDirectory ) )
        {
            initialDirectory = options.InitialDirectory;
        }
        else if( !string.IsNullOrWhiteSpace( options.InitialFileName ) )
        {
            initialDirectory = Path.GetDirectoryName( options.InitialFileName ) ?? string.Empty;
        }
        
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
      
        vm.Filters = options.Filters
            .Select( filter => new FilePickerFileType( filter.Name )
            {
                Patterns = filter.Extensions.Select( p => $"*.{p}" ).ToArray()
            } )
            .ToArray();

        var windowVM = new BaseDialogWindowViewModel
        {
            FileDialogVM =  vm,
            OnLoaded = ReactiveCommand.Create<object>(sender => {})!
        };

        _mainWindow.DataContext = windowVM;
        
        var selection = vm.FileList.SelectedEntities ?? Array.Empty<FileEntityModelBase>();

        return _mainWindow.ShowDialog<string[]>(parent);
    }
}