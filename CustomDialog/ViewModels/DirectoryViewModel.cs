using System;
using System.Drawing;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using ReactiveUI;
using Brush = Avalonia.Media.Brush;

namespace CustomDialog.ViewModels;

public class DirectoryViewModel : ViewModelBase
{
    private Random rnd = new();
    private string _text;
    public string Text => _text;

    public DirectoryViewModel()
    {
        _text = rnd.Next(0, 100).ToString();
    }
}