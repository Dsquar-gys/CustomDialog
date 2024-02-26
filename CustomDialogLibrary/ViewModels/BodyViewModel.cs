using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.History;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class BodyViewModel : ViewModelBase, IDisposable
{
    #region Private Fields

    private string? _filePath;
    private FileEntityModel? _selectedFileEntity;
    private readonly DirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private FileDialogFilter? _filter;
    private BodyTemplate? _currentStyle;
    private readonly SourceCache<FileEntityModel, string> _dataSource = new(entity => entity.Name);
    private readonly ReadOnlyObservableCollection<FileEntityModel> _outerCollection;

    #endregion
    
    #region Properties
    
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
    
    public string? FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }

    public ReadOnlyObservableCollection<FileEntityModel> OuterCollection => _outerCollection;
    
    public FileEntityModel? SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    #endregion
    
    #region Commands

    public ReactiveCommand<FileDialogFilter, Unit> ChangeFilterCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveForwardCommand { get; }

    #endregion
    
    public BodyViewModel()
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
            .Filter(x => Filter is null || (Filter.Extensions.Contains(x.Extension) ||
                                            string.IsNullOrWhiteSpace(x.Extension) ||
                                            Filter.Extensions is [""]))
            // Sorting folders first
            .Sort(SortExpressionComparer<FileEntityModel>.Ascending(x => x.GetType().ToString()))
            // Binding to inner collection
            .Bind(out _outerCollection)
            .Subscribe();
        
        // Update data on filter changed
        this.WhenAnyValue(x => x.Filter)
            .Subscribe(_ => _dataSource.Refresh());
        
        // Opens entity on FilePath changed
        this.WhenAnyValue(x => x.FilePath)
            .Subscribe(Open);
    }
    
    #region Commands Methods

    /// <summary>
    /// Changes filter for file extensions
    /// </summary>
    /// <param name="filter">New filter</param>
    private void ChangeFilter(FileDialogFilter filter)
    {
        Filter = filter;
        Console.WriteLine("Filter changed in BodyVM");
    }
    
    /// <summary>
    /// Displays entity on body or opens it in a new process
    /// </summary>
    /// <param name="path">Path for required to open entity</param>
    private async void Open(string? path)
    {
        if (!File.Exists(path) && !Directory.Exists(path)) return;
        switch (File.GetAttributes(path))
        {
            case FileAttributes.Directory:
            case FileAttributes.Directory | FileAttributes.ReadOnly:
                _history.Add(path);
                await OpenDirectoryAsync();
                break;
            default:
                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = path;
                    process.StartInfo.CreateNoWindow = false;
                    process.Start();
                }
                break;
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
        Console.WriteLine("Start thread: {0}", Environment.CurrentManagedThreadId);
        
        // Cancel running task
        await _tokenSource.CancelAsync();

        // Creating new cancellation source and token
        _tokenSource = new();
        _token = _tokenSource.Token;
        
        var directoryInfo = new DirectoryInfo(FilePath!);
        // Awaiting task that pulls content from directory and returns collection
        await Task.Run(() =>
        {
            Console.WriteLine("Awaited task in thread: {0}", Environment.CurrentManagedThreadId);
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
                pulling.Add(new DirectoryModel(directory));
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
                pulling.Add(new FileModel(file));
            }

            return pulling;
        }, _token).ContinueWith(x =>       // Continuation in UI context
        {
            // Convert pulled collection to source data
            _dataSource.Edit(innerCollection =>
            {
                innerCollection.Load(x.Result);
            });
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