using System;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialog.Models;

public static class ImageHelper
{
    private const string NotFoundIcon = "unknown";
    public static string DefaultIconName => NotFoundIcon;

    public static Bitmap DefaultIcon => new Bitmap(AssetLoader.Open(
        new Uri(Path.Combine("avares://CustomDialog/Assets/Icons", NotFoundIcon + ".png"))));

    public static Bitmap LoadFromResource(Uri resourceUri)
    {
        Bitmap icon;
        try
        {
            icon = new Bitmap(AssetLoader.Open(resourceUri));
        }
        catch (Exception e)
        {
            Console.WriteLine("Icon for {0} not found...", resourceUri.AbsolutePath.Split('/').Last());
            icon = DefaultIcon;
        }

        return icon;
    }
        
    public static Bitmap LoadFromResource(string resourcePath) =>
        LoadFromResource(new Uri(resourcePath));
}