using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.History;
using CustomDialogLibrary.Interfaces;
using DynamicData;
using ReactiveUI;

namespace DemoApp.ViewModels;

public class BodyViewModel : ViewModelBase, IBody
{
    #region Private Fields

    private string? _filePath;
    private FileEntityModel? _selectedFileEntity;
    private readonly IDirectoryHistory _history;
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    private readonly ReadOnlyObservableCollection<FileEntityModel> _outerDirectoryContent;
    private FileDialogFilter _filter;
    private BodyTemplate? _currentStyle;

    #endregion
    
    #region Properties

    public FileDialogFilter Filter
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
    
    public ReadOnlyObservableCollection<FileEntityModel> OuterDirectoryContent => _outerDirectoryContent;
    
    public SourceCache<FileEntityModel, string> DirectoryData { get; } =
        new(entity => entity.Title);
    
    public FileEntityModel? SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    #endregion
    
    #region Commands

    public ReactiveCommand<FileDialogFilter, Unit> ChangeFilterReactiveCommand { get; }
    public ReactiveCommand<ILoadable, Unit> ChangeSelectedReactiveCommand { get; }
    public ReactiveCommand<object?, Unit> OpenReactiveCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveBackReactiveCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveForwardReactiveCommand { get; }

    #endregion
    
    public BodyViewModel()
    {
        _history = DirectoryHistory.DefaultPage;
        
        ChangeSelectedReactiveCommand = ReactiveCommand.Create<ILoadable>(ChangeSelected);
        OpenReactiveCommand = ReactiveCommand.Create<object?>(Open);
        ChangeFilterReactiveCommand = ReactiveCommand.Create<FileDialogFilter>(ChangeFilter);
        MoveBackReactiveCommand = ReactiveCommand.Create(OnMoveBack, _history.CanMoveBack);
        MoveForwardReactiveCommand = ReactiveCommand.Create(OnMoveForward, _history.CanMoveForward);
        
        FilePath = _history.Current.DirectoryPath;
        
        DirectoryData.Connect()
            .Filter(x => Filter.Extensions.Contains(x.Extension) ||
                         string.IsNullOrWhiteSpace(x.Extension) ||
                         Filter.Extensions is [""])
            .Bind(out _outerDirectoryContent)
            .Subscribe();
    }
    
    #region Commands Methods

    private void ChangeFilter(FileDialogFilter filter)
    {
        Filter = filter;
        Console.WriteLine("Filter changed in BodyVM");
        
        DirectoryData.Refresh();
    }
    
    private void ChangeSelected(ILoadable directory)
    {
        FilePath = directory.FullPath;
        _history.Add(directory.FullPath, directory.Title);
    }
    
    private void Open(object? sender)
    {
        switch (sender)
        {
            case ILoadable loadable:
                FilePath = loadable.FullPath;
                OpenDirectoryAsync();
                break;
            case FileModel file:
                Console.WriteLine("BodyViewModel --> Open --> File opening...");
                break;
            case TextBox textBox:
                FilePath = textBox.Text;
                OpenDirectoryAsync();
                break;
        }
    }

    private void OnMoveForward()
    {
        _history.MoveForward();
        FilePath = _history.Current.DirectoryPath;
    }

    private void OnMoveBack()
    {
        _history.MoveBack();
        FilePath = _history.Current.DirectoryPath;
    }

    #endregion
    
    #region Private Methods
    
    private async Task OpenDirectoryAsync()
    {
        Console.WriteLine("Start thread: {0}", Environment.CurrentManagedThreadId);
        
        await _tokenSource.CancelAsync();
        DirectoryData.Clear();
        await Task.Delay(10);

        _tokenSource = new();
        _token = _tokenSource.Token;
        
        var directoryInfo = new DirectoryInfo(FilePath!);
        // Task 2
        await Task.Run(() =>
        {
            Console.WriteLine("Awaited task in thread: {0}", Environment.CurrentManagedThreadId);
            List<FileEntityModel> pulling = [];
            
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
            DirectoryData.AddOrUpdate(x.Result);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    #endregion
}