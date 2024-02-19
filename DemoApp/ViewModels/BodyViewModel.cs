using System;
using System.Collections.Generic;
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
    private FileDialogFilter _filter;
    private BodyTemplate? _currentStyle;

    #endregion
    
    #region Properties

    /// <summary>
    /// Gets or sets current <see cref="FileDialogFilter"/>
    /// </summary>
    public FileDialogFilter Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }
    
    /// <summary>
    /// Gets or sets display style for <see cref="IBody"/>
    /// </summary>
    public BodyTemplate? CurrentStyle
    {
        get => _currentStyle;
        set
        {
            value!.LinkCollection(this);
            this.RaiseAndSetIfChanged(ref _currentStyle, value);
        }
    }
    
    /// <summary>
    /// Gets or sets current directory path
    /// </summary>
    public string? FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
    }

    /// <summary>
    /// Data source from current directory
    /// </summary>
    public SourceCache<FileEntityModel, string> DirectoryData { get; } =
        new(entity => entity.Title);
    
    /// <summary>
    /// Selected object
    /// </summary>
    public FileEntityModel? SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    #endregion
    
    #region Commands

    public ReactiveCommand<FileDialogFilter, Unit> ChangeFilterReactiveCommand { get; }
    public ReactiveCommand<object, Unit> ChangeSelectedReactiveCommand { get; }
    public ReactiveCommand<object?, Unit> OpenReactiveCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveBackReactiveCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveForwardReactiveCommand { get; }

    #endregion
    
    public BodyViewModel()
    {
        // History init
        _history = DirectoryHistory.DefaultPage;
        
        // Commands init
        ChangeSelectedReactiveCommand = ReactiveCommand.Create<object>(ChangeSelected);
        OpenReactiveCommand = ReactiveCommand.Create<object?>(Open);
        ChangeFilterReactiveCommand = ReactiveCommand.Create<FileDialogFilter>(ChangeFilter);
        MoveBackReactiveCommand = ReactiveCommand.Create(OnMoveBack, _history.CanMoveBack);
        MoveForwardReactiveCommand = ReactiveCommand.Create(OnMoveForward, _history.CanMoveForward);
        
        // Directory we're in currently
        FilePath = _history.Current.DirectoryPath;

        // Update data on filter changed
        this.WhenAnyValue(x => x.Filter)
            .Subscribe(_ => DirectoryData.Refresh());
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
    /// Changes the current directory to be displayed
    /// </summary>
    /// <param name="sender">Directory, which content should be displayed</param>
    private void ChangeSelected(object sender)
    {
        switch (sender)
        {
            case ILoadable directory:
                FilePath = directory.FullPath;
                _history.Add(directory.FullPath, directory.Title);
                break;
            case FileModel:
                Open(sender);
                break;
            default: throw new NotImplementedException("Sender type is not implemented yet");
        }
    }
    
    /// <summary>
    /// Displays <see cref="ILoadable"/> on body or opens <see cref="FileModel"/>
    /// </summary>
    /// <param name="sender">Object that should be opened</param>
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
                throw new NotImplementedException("File opening is not implemented yet");
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
    
    /// <summary>
    /// Pulls content (files and folders) from current directory
    /// </summary>
    /// <returns>
    /// A task that on completion updates content both in cases of successful completion and cancellation
    /// </returns>
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