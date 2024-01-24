using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.Entities;
using CustomDialog.ViewModels.History;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class BodyViewModel : ViewModelBase
{
    #region Private Fields
    
    private FileEntityViewModel _selectedFileEntity;
    private readonly IDirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;

    #endregion
    
    #region Properties
    
    public string FilePath { get; set; }
    public string Name { get; set; }
    public ObservableCollection<FileEntityViewModel> DirectoryContent { get; set; } = new();
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
        _history = new DirectoryHistory("/home", "Home");

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
        if (parameter is DirectoryViewModel directoryViewModel)
        {
            FilePath = directoryViewModel.FullName;
            Name = directoryViewModel.Name;

            _history.Add(FilePath, Name);

            OpenDirectoryAsync();
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
        // TO DELETE
        await Task.Delay(10);

        _tokenSource = new();
        _token = _tokenSource.Token;
            
        var directoryInfo = new DirectoryInfo(FilePath);
        // Task 2
        await Task.Run(() =>
        {
            Console.WriteLine("Awaited task in thread: {0}", Environment.CurrentManagedThreadId);
            
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled in thread: {0}", Environment.CurrentManagedThreadId);
                    return;
                }

                // TO DELETE
                //await Task.Delay(10);
                DirectoryContent.Add(new DirectoryViewModel(directory));
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled in thread: {0}", Environment.CurrentManagedThreadId);
                    return;
                }
                DirectoryContent.Add(new FileViewModel(file));
            }
        }, _token);
    }
    
    #endregion
}