using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CustomDialog.Models.Entities;
using CustomDialog.Models.Interfaces;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.History;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class BodyViewModel : ViewModelBase
{
    #region Private Fields

    private string? _filePath;
    private string? _name;
    private FileEntityModel _selectedFileEntity;
    private readonly IDirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private ObservableCollection<FileEntityModel> _directoryContent = new();
    private ISpecificFileViewModel _selectedStyle;

    #endregion
    
    #region Properties

    public ISpecificFileViewModel SelectedStyle
    {
        get => _selectedStyle;
        set => this.RaiseAndSetIfChanged(ref _selectedStyle, value);
    }
    
    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    public string? FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }
    public ObservableCollection<FileEntityModel> DirectoryContent
    {
        get => _directoryContent;
        set => this.RaiseAndSetIfChanged(ref _directoryContent, value);
    }
    public FileEntityModel SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    #endregion
    
    #region Commands

    public DelegateCommand ChangeSelectedCommand { get; }
    
    public DelegateCommand OpenCommand { get; }

    public DelegateCommand MoveBackCommand { get; }

    public DelegateCommand MoveForwardCommand { get; }

    #endregion

    public BodyViewModel()
    {
        _history = DirectoryHistory.DefaultPage;

        ChangeSelectedCommand = new DelegateCommand(ChangeSelected);
        OpenCommand = new DelegateCommand(Open);
        MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
        MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);
        
        Name = _history.Current.DirectoryPathName;
        FilePath = _history.Current.DirectoryPath;
        
        _history.HistoryChanged += History_HistoryChanged;
        
        _token = _tokenSource.Token;
    }
    
    #region Commands Methods

    private void ChangeSelected(object parameter)
    {
        if (parameter is ILoadable directory)
        {
            FilePath = directory.FullPath;
            _history.Add(directory.FullPath, directory.Title);
        }
    }
    
    private void Open(object parameter)
    {
        if (parameter is ILoadable loadable)
        {
            FilePath = loadable.FullPath;
            Name = loadable.Title;

            OpenDirectoryAsync();
        }

        if (parameter is FileModel file)
        {
            Console.WriteLine("BodyViewModel --> Open --> File opening...");
        }

        if (parameter is TextBox textBox)
        {
            FilePath = textBox.Text;
            Name = new string(textBox.Text.Skip(1 + textBox.Text.LastIndexOf('/')).ToArray());
            
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
    }

    private bool OnCanMoveBack(object obj) => _history.CanMoveBack;

    private void OnMoveBack(object obj)
    {
        _history.MoveBack();

        var current = _history.Current;

        FilePath = current.DirectoryPath;
        Name = current.DirectoryPathName;
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
        await Task.Delay(10);

        _tokenSource = new();
        _token = _tokenSource.Token;
        
        var directoryInfo = new DirectoryInfo(FilePath);
        // Task 2
        await Task.Run(() =>
        {
            Console.WriteLine("Awaited task in thread: {0}", Environment.CurrentManagedThreadId);
            ObservableCollection<FileEntityModel> pulling = new();
            
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                pulling.Add(new DirectoryModel(directory));
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                pulling.Add(new FileModel(file));
            }

            return pulling;
        }, _token).ContinueWith(x =>
        {
            DirectoryContent = x.Result;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    #endregion
}