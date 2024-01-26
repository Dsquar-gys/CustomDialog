using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CustomDialog.Models;

namespace CustomDialog.ViewModels.Entities;

public abstract class FileEntityViewModel : ViewModelBase, IImagable
{
    public string Title { get; set; }
    public string FullPath { get; set; }
    public IImage? Icon { get; }

    protected FileEntityViewModel(string path, string? title = null)
    {
        Title = title ?? path;
        FullPath = path;

        string type = "";

        if (this is FileViewModel)
            Icon = new Bitmap(AssetLoader.Open(new Uri("avares://CustomDialog/Assets/Icons/file.png")));
        if (this is DirectoryViewModel)
            Icon = new Bitmap(AssetLoader.Open(new Uri("avares://CustomDialog/Assets/Icons/folder.png")));
        //Icon = new Bitmap(AssetLoader.Open(new Uri("avares://CustomDialog/Assets/Icons/folder.png")));
    }
}