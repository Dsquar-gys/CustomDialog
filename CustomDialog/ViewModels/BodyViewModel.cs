using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CustomDialog.Models;
using CustomDialog.Models.Entities;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.History;
using CustomDialog.Views.DataTemplates;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class BodyViewModel : ViewModelBase
{
    #region Private Fields

    private string _filePath;
    private string _name;
    private FileEntityModel _selectedFileEntity;
    private readonly IDirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private ObservableCollection<FileEntityModel> _directoryContent = new();
    private TemplateStyle _currentTemplateStyle;
    private IDataTemplate _www = new WrapPanelTemplate();

    #endregion
    
    #region Properties

    public SpecificFileViewModel Port => new();

    public IDataTemplate WWW
    {
        get => _www;
        set => this.RaiseAndSetIfChanged(ref _www, value);
    }
    
    public TemplateStyle CurrentTemplateStyle
    {
        get => _currentTemplateStyle;
        set
        {
            SpecificFileViewModel.CommonTemplate = value switch
            {
                TemplateStyle.WrapPanel => new WrapPanelTemplate(),
                TemplateStyle.DataGrid => new DataGridTemplate(),
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown Template Style")
            };
            this.RaiseAndSetIfChanged(ref _currentTemplateStyle, value);
        }
    }
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    public string FilePath
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
        
        SpecificFileViewModel.CommonTemplate = new WrapPanelTemplate();
        
        Name = _history.Current.DirectoryPathName;
        FilePath = _history.Current.DirectoryPath;
        
        _history.HistoryChanged += History_HistoryChanged;
        
        _token = _tokenSource.Token;
    }
    
    #region Command Methods

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
            FileEntityModel? _entity;
            
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }

                if (new SpecificFileViewModel().TryToCreateFileEntry(directory, out _entity))
                    pulling.Add(_entity);
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (_token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                    return pulling;
                }
                if (new SpecificFileViewModel().TryToCreateFileEntry(file, out _entity))
                    pulling.Add(_entity);
            }

            return pulling;
        }, _token).ContinueWith(x =>
        {
            DirectoryContent = x.Result;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    #endregion
}