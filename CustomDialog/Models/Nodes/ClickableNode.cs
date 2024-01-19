using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CustomDialog.ViewModels;

namespace CustomDialog.Models.Nodes;

public class ClickableNode : INode
{
    public string Title { get; }
    public bool Selectable { get; } = true;
    public IImage? Icon { get; }
    public string DirectoryPath { get; }

    public ClickableNode(string title)
    {
        Title = title;
        try
        {
            Icon = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://CustomDialog/Assets", "Icons", Title.ToLower() + ".png"))));
        }
        catch (FileNotFoundException e)
        {
            Icon = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://CustomDialog/Assets", "Icons", "check.png"))));
            Console.WriteLine("Icon for \"{0}\" not found", Title);
        }

        DirectoryPath = Path.Combine("/", Title.ToLower());
    }
}