using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CustomDialog.Models;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.Entities;
using CustomDialog.ViewModels.History;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class BodyViewModel : ViewModelBase
{
    #region Private Fields

    private string _filePath;
    private FileEntityViewModel _selectedFileEntity;
    private readonly IDirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private ObservableCollection<FileEntityViewModel> _directoryContent = new();

    #endregion
    
    #region Properties
    
    
    public string Name { get; set; }
    public string FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }
    public ObservableCollection<FileEntityViewModel> DirectoryContent
    {
        get => _directoryContent;
        set => this.RaiseAndSetIfChanged(ref _directoryContent, value);
    }
    public FileEntityViewModel SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    #endregion
    
    #region Commands

    public DelegateCommand OpenCommand { get; }

    public DelegateCommand MoveBackCommand { get; }

    public DelegateCommand MoveForwardCommand { get; }

    #endregion

    public BodyViewModel()
    {
        _history = DirectoryHistory.DefaultPage;

        OpenCommand = new DelegateCommand(Open);
        MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
        MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);
        
        Name = _history.Current.DirectoryPathName;
        FilePath = _history.Current.DirectoryPath;

        OpenDirectoryAsync();
        
        _history.HistoryChanged += History_HistoryChanged;
        
        _token = _tokenSource.Token;
    }
    
    #region Commands Methods

    private void Open(object parameter)
    {
        if (parameter is ILoadable loadable)
        {
            FilePath = loadable.FullPath;
            Name = loadable.Title;

            _history.Add(FilePath, Name);

            OpenDirectoryAsync();
        }

        if (parameter is FileViewModel file)
        {
            Console.WriteLine("BodyViewModel --> Open --> File opening...");
        }
    }

    private bool OnCanMoveForward(object obj) => _history.CanMoveForward;

    private void OnMoveForward(object obj)
    {
        _history.MoveForward();

        var current = _history.Current;

        FilePath = current.DirectoryPath;
        Name = current.DirectoryPathName;

        OpenDirectoryAsync();
    }

    private bool OnCanMoveBack(object obj) => _history.CanMoveBack;

    private void OnMoveBack(object obj)
    {
        _history.MoveBack();

        var current = _history.Current;

        FilePath = current.DirectoryPath;
        Name = current.DirectoryPathName;

        OpenDirectoryAsync();
    }

    #endregion
    
    #region Private Methods

    private void History_HistoryChanged(object sender, EventArgs e)
    {
        MoveBackCommand?.RaiseCanExecuteChanged();
        MoveForwardCommand?.RaiseCanExecuteChanged();
    }
    
    private async Task OpenDirectoryAsync()
    {
        Console.WriteLine("Start thread: {0}", Environment.CurrentManagedThreadId);
        
        await _tokenSource.CancelAsync();
        DirectoryContent.Clear();
        await Task.Delay(10); // TO DELETE

        _tokenSource = new();
        _token = _tokenSource.Token;
            
        var directoryInfo = new DirectoryInfo(FilePath);
        // Task 2
        await Task.Run(() =>
        {
            Console.WriteLine("Awaited task in thread: {0}", Environment.CurrentManagedThreadId);
            ObservableCollection<FileEntityViewModel> pulling = new();
            
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                //Task.Delay(1000).Wait(); // TO DELETE
                pulling.Add(new DirectoryViewModel(directory));
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                pulling.Add(new FileViewModel(file));
            }

            return pulling;
        }, _token).ContinueWith(x =>
        {
            Console.WriteLine("Continuation in thread: {0}", Environment.CurrentManagedThreadId);
            DirectoryContent = x.Result;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    #endregion
}