using System;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Diagnostics;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using DemoApp.ViewModels;
using DemoApp.Views;
using ReactiveUI;

namespace DemoApp;

public class OpenFileDialog : ReactiveObject
{
    private readonly Window? _mainWindow;
    private readonly Subject<string> _pending;
    
    public OpenFileDialog()
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
            _pending.OnNext( fileName );
            
            DefaultSettings = DefaultSettings with
            {
                InitialDirectory = Path.GetDirectoryName( fileName ) ??
                                   Environment.GetFolderPath( Environment.SpecialFolder.Personal )
            };
        }
    }

    private async Task<string[]> ShowDialogAsync( OpenFileDialogOptions options, Window parent )
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
            options.AllowMultiple,
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
            FileDialogVM =  vm
        };

        _mainWindow.DataContext = windowVM;
        
        string[] selection = [];
        
        vm.OkCmd.Subscribe(_ =>
        {
            selection = vm.FileList.SelectedEntities!.Select(x => x.FullPath).ToArray();
            _mainWindow.Close();
        });
        vm.CancelCmd.Subscribe(_ =>
        {
            _mainWindow.Close();
        });

        await _mainWindow.ShowDialog(parent);

        return selection;
    }
}