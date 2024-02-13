using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialogLibrary;

public static class ImageHelper
{
    public static string DefaultIconName { get; private set; } = "unknown";

    public static string AssetsPath { get; private set; } = "avares://CustomDialogLibrary/BakedAssets";

    public static Bitmap DefaultIcon => new (AssetLoader.Open(
        new Uri(Path.Combine(AssetsPath,  DefaultIconName + ".png"))));

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

    public static void SetPath(Uri IconsUri, string DefaultImageName)
    {
        AssetsPath = IconsUri.OriginalString;
        DefaultIconName = DefaultImageName;
    }
}