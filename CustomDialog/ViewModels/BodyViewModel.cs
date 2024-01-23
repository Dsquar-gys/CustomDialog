using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CustomDialog.ViewModels.Entities;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class BodyViewModel : ViewModelBase
{
    private readonly Random _rnd = new();
    private FileEntityViewModel _selectedFileEntity;

    public ObservableCollection<FileEntityViewModel> DirectoryContent { get; set; } = new();

    public FileEntityViewModel SelectedFileEntity
    {
        get => _selectedFileEntity;
        set => this.RaiseAndSetIfChanged(ref _selectedFileEntity, value);
    }

    public string FilePath { get; set; }

    public BodyViewModel()
    {
        FilePath = _rnd.Next(0, 100).ToString();

        foreach (var localDrive in Directory.GetLogicalDrives())
            DirectoryContent.Add(new DirectoryViewModel(localDrive));
        
        _token = _tokenSource.Token;
    }
    
    private CancellationTokenSource _tokenSource = new();
    private CancellationToken _token;
    public async Task OpenAsync(object parameter)
    {
        Console.WriteLine("Start thread: {0}", Environment.CurrentManagedThreadId);
        if (parameter is DirectoryViewModel directoryViewModel)
        {
            FilePath = directoryViewModel.FullName;

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
    }
}