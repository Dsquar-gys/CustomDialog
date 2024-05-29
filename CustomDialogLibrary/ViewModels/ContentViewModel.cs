using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.History;
using CustomDialogLibrary.Interfaces;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class ContentViewModel : ViewModelBase, IDisposable
{
    #region Private Fields

    private string _filePath = string.Empty;
    private readonly DirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private FileDialogFilter? _filter;
    private BodyTemplate? _currentStyle;
    private readonly ISpecificFileViewModel _specificFileViewModel;
    private readonly SourceCache<FileEntityModel, string> _dataSource = new(entity => entity.Name);
    private readonly ReadOnlyObservableCollection<FileEntityModel> _outerCollection;
    private bool _toClose;

    #endregion
    
    #region Properties
    
    public bool ToClose
    {
        get => _toClose;
        private set => this.RaiseAndSetIfChanged(ref _toClose, value);
    }
    
    public required ISpecificFileViewModel SpecificFileViewModel
    {
        get => _specificFileViewModel;
        init => this.RaiseAndSetIfChanged(ref _specificFileViewModel, value);
    }
    
    public FileDialogFilter? Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }
    
    public BodyTemplate? CurrentStyle
    {
        get => _currentStyle;
        set => this.RaiseAndSetIfChanged(ref _currentStyle, value);
    }
    
    public string FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }

    public ReadOnlyObservableCollection<FileEntityModel> OuterCollection => _outerCollection;
    public ObservableCollection<FileEntityModel> SelectedEntities { get; } = new();

    #endregion
    
    #region Commands

    public ReactiveCommand<FileDialogFilter, Unit> ChangeFilterCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveForwardCommand { get; }

    #endregion
    
    public ContentViewModel()
    {
        // History init
        _history = DirectoryHistory.DefaultPage;
        
        // Commands init
        ChangeFilterCommand = ReactiveCommand.Create<FileDialogFilter>(ChangeFilter);
        MoveBackCommand = ReactiveCommand.Create(OnMoveBack, _history.CanMoveBack);
        MoveForwardCommand = ReactiveCommand.Create(OnMoveForward, _history.CanMoveForward);
        
        // Directory we're in currently
        FilePath = _history.Current.Path;

        _dataSource.Connect()
            // Filtering proper extensions
            .Filter(x => Filter is null || Filter.Extensions.Contains(x.Extension) ||
                                            string.IsNullOrWhiteSpace(x.Extension) ||
                                            Filter.Extensions is ["*"])
            // Sorting folders first
            .Sort(SortExpressionComparer<FileEntityModel>.Ascending(x => x.GetType().ToString()))
            // Binding to inner collection
            .Bind(out _outerCollection)
            .Subscribe();
        
        // Update data on filter changed
        this.WhenAnyValue(x => x.Filter)
            .Subscribe(_ => _dataSource.Refresh());
        
        // Opens entity on any FilePath change
        this.WhenAnyValue(x => x.FilePath)
            .Subscribe(path =>
            {
                if (!File.Exists(path) && !Directory.Exists(path)) return;
                
                if (File.Exists(path))
                {
                    var toSelect = OuterCollection.Where(x => x.FullPath == path);

                    if (OuterCollection.All(x => x.FullPath != path)) return;

                    SelectedEntities.Clear();
                    SelectedEntities.Add(toSelect.First());
                }
                
                Open(path);
            });
    }
    
    #region Commands Methods

    /// <summary>
    /// Changes filter for file extensions
    /// </summary>
    /// <param name="filter">New filter</param>
    private void ChangeFilter(FileDialogFilter filter)
    {
        Filter = filter;
        Console.WriteLine("Filter changed in ContentVm");
    }
    
    /// <summary>
    /// Displays entity on body or returns file by path
    /// </summary>
    /// <param name="path">Path for required to open entity</param>
    /// <remarks>If path is NULL it checks property `FilePath`</remarks>
    private async void Open(string path)
    {
        var attributes = File.GetAttributes(path).ToString().Split(new[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);

        if (attributes.Contains(FileAttributes.Directory.ToString()))
        {
            _history.Add(path);
            await OpenDirectoryAsync();
        }
        else
        {
            ToClose = true;
        }
    }

    private void OnMoveForward()
    {
        _history.MoveForward();
        FilePath = _history.Current.Path;
    }

    private void OnMoveBack()
    {
        _history.MoveBack();
        FilePath = _history.Current.Path;
    }

    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Pulls content (files and folders) from current directory
    /// </summary>
    /// <returns>
    /// A task that on completion updates content both in cases of successful completion and cancellation
    /// </returns>
    private async Task OpenDirectoryAsync()
    {
        // Cancel running task
        await _tokenSource.CancelAsync();

        // Creating new cancellation source and token
        _tokenSource = new();
        _token = _tokenSource.Token;
        
        var directoryInfo = new DirectoryInfo(FilePath);
        
        // Awaiting task that pulls content from directory and returns collection
        await Task.Run(() =>
        {
            // Result collection
            List<FileEntityModel> pulling = [];
            
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                // Checking for cancellation
                if (_token.IsCancellationRequested)         // Safe cancellation (without exception)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                // Filling collection
                if (SpecificFileViewModel.TryToCreateFileEntry(directory, out var model))
                    pulling.Add(model);
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                // Checking for cancellation
                if (_token.IsCancellationRequested)         // Safe cancellation (without exception)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }

                // Filling collection
                if (SpecificFileViewModel.TryToCreateFileEntry(file, out var model))
                    pulling.Add(model);
            }

            return pulling;
        }, _token).ContinueWith(x =>       // Continuation in UI context
        {
            // Convert pulled collection to source data
            if (x.IsCompletedSuccessfully)
                _dataSource.Edit(innerCollection =>
                {
                    innerCollection.Load(x.Result);
                });
            else Console.WriteLine("Exception occured in OpenDirectoryAsync");
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    #endregion

    public void Dispose()
    {
        _tokenSource.Dispose();
        _dataSource.Dispose();
        ChangeFilterCommand.Dispose();
        MoveBackCommand.Dispose();
        MoveForwardCommand.Dispose();
    }
}