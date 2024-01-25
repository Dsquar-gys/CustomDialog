using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialog.Models.Nodes;

public class ClickableNode : INode, ILoadable, IImagable
{
    public string FullPath { get; }
    public string Title { get; }
    public bool Selectable => true;
    public IImage? Icon { get; }

    public ClickableNode(string path, string title)
    {
        Title = title;
        FullPath = path;
        try
        {
            Icon = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://CustomDialog/Assets", "Icons", Title.ToLower() + ".png"))));
        }
        catch (FileNotFoundException e)
        {
            Icon = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://CustomDialog/Assets", "Icons", "check.png"))));
            Console.WriteLine("Icon for \"{0}\" not found", Title);
        }
    }
}