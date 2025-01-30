using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Platform.Storage;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.Models;
using DynamicData;
using Microsoft.Extensions.FileSystemGlobbing;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class FolderListingVM : ViewModelBase, IDisposable
{
    #region Private Fields

    private readonly SourceCache<FileEntityModelBase, string> _dataSource = new(entity => entity.Name);
    
    private readonly IDialogCustomizationsFactory _dialogCustomizationsFactory;
    
    private readonly ReadOnlyObservableCollection<FileEntityModelBase> _displayList;

    private FolderListingTemplate? _currentStyle;

    private FilePickerFileType _filter = new( "Все файлы" ) { Patterns = ["*.*"] };
    
    private Matcher _matcher = new();
    
    private IList<FileEntityModelBase>? _selectedEntities = new List<FileEntityModelBase>();
    
    private CancellationTokenSource? _tokenSource;

    private bool _showHidden;

    #endregion
    
    public FolderListingVM(IObservable<string> folderSelection)
    {
        _dialogCustomizationsFactory =
            new DefaultFileDialogCustomizationsFactory( [new WrapPanelTemplate(), new DataGridTemplate()] );

        var filterChanged = this.WhenAnyValue(property1: vm => vm.Filter, property2: vm => vm.ShowHidden)
            .Select(CreateFilterPredicate);
        
        _dataSource.Connect()
            // Filtering proper extensions
            .Filter(filterChanged)
            // Sorting folders first
            .Sort(new FileEntityComparer())
            .ObserveOn(RxApp.MainThreadScheduler)
            // Binding to inner collection
            .Bind(out _displayList)
            .Subscribe();

        folderSelection.Select(path => Observable.FromAsync(_ => OnNext(path)))
            .Concat()
            .Subscribe();
    }
    
    #region Properties
    
    public bool ShowHidden
    {
        get => _showHidden;
        set => this.RaiseAndSetIfChanged( ref _showHidden, value );
    }
    
    public required IDialogCustomizationsFactory DialogCustomizationsFactory
    {
        get => _dialogCustomizationsFactory;
        init => this.RaiseAndSetIfChanged(ref _dialogCustomizationsFactory, value);
    }
    
    public bool AllowMultipleSelection { get; init; }
    
    public FilePickerFileType Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged( ref _filter, value );
    }
    
    public FolderListingTemplate? CurrentStyle
    {
        get => _currentStyle;
        set => this.RaiseAndSetIfChanged(ref _currentStyle, value);
    }
    
    public ReadOnlyObservableCollection<FileEntityModelBase> DisplayList => _displayList;

    public IList<FileEntityModelBase>? SelectedEntities
    {
        get => _selectedEntities;
        set => this.RaiseAndSetIfChanged( ref _selectedEntities, value );
    }

    #endregion
    
    #region Private Methods
    
    private Func<FileEntityModelBase, bool> CreateFilterPredicate( ( FilePickerFileType filter, bool showHidden ) val )
    {
        if( val.filter.Patterns is null or ["*.*"] )
        {
            if( val.showHidden )
            {
                return _ => true;
            }

            return e => !e.Hidden;
        }

        _matcher = new Matcher( StringComparison.OrdinalIgnoreCase );
        _matcher.AddIncludePatterns( val.filter.Patterns );

        if( val.showHidden )
        {
            return e => e is DirectoryModel || _matcher.Match( e.Name ).HasMatches;
        }
    
        return e =>  !e.Hidden && ( e is DirectoryModel || _matcher.Match( e.Name ).HasMatches );
    }
    
    private async Task OnNext( string path )
    {
        if( string.IsNullOrWhiteSpace( path ) )
        {
            return;
        }

        // Cancel running task
        if( _tokenSource is not null )
        {
            await _tokenSource.CancelAsync();
            _tokenSource.Dispose();
            _tokenSource = null;
        }

        try
        {
            if( ( File.GetAttributes( path ) & FileAttributes.Directory ) == 0 )
            {
                path = Path.GetDirectoryName( path ) ?? throw new DirectoryNotFoundException( $"Папка {path} не найдена" );
            }

            if( !Directory.Exists( path ) )
            {
                throw new DirectoryNotFoundException( $"Папка {path} не найдена" );
            }

            var directoryInfo = new DirectoryInfo( path );

            // Creating new cancellation source and token
            _tokenSource = new CancellationTokenSource();

            var data = await OpenDirectoryAsync( directoryInfo, _tokenSource.Token );

            _dataSource.Edit( ed => { ed.Load( data ); } );
        }
        catch( Exception ex )
        {
            await Console.Error.WriteLineAsync( ex.Message );
        }
    }
    
    /// <summary>
    /// Pulls content (files and folders) from current directory
    /// </summary>
    /// <returns>
    /// A task that on completion updates content both in cases of successful completion and cancellation
    /// </returns>
    private async Task<FileEntityModelBase[]> OpenDirectoryAsync( DirectoryInfo directoryInfo, CancellationToken ct )
    {
        var toProcess = directoryInfo.EnumerateFileSystemInfos()
            .Select( file =>  DialogCustomizationsFactory.TryToCreateFileEntry( file ) );

        var data = await Task.WhenAll( toProcess ).WaitAsync( ct );

        return data.Where( x => x is not null ).ToArray()!;
    }
    
    #endregion

    public void Dispose()
    {
        _tokenSource?.Dispose();
        _dataSource.Dispose();
    }
}