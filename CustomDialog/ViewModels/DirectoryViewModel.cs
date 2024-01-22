using System;
using System.Collections;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class DirectoryViewModel : ViewModelBase
{
    private readonly Random _rnd = new();
    private string _text;
    private IEnumerable _directoryContent;

    public IEnumerable DirectoryContent
    {
        get => _directoryContent;
        set => this.RaiseAndSetIfChanged(ref _directoryContent, value);
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    public DirectoryViewModel()
    {
        Text = _rnd.Next(0, 100).ToString();
    }
}