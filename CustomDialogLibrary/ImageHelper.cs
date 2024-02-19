using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialogLibrary;

public static class ImageHelper
{
    /// <summary>
    /// Name of baked default image name
    /// </summary>
    public static string DefaultIconName { get; private set; } = "unknown";

    /// <summary>
    /// Path for baked assets
    /// </summary>
    public static string AssetsPath { get; private set; } = "avares://CustomDialogLibrary/BakedAssets";

    /// <summary>
    /// Gets default image for icon
    /// </summary>
    public static Bitmap DefaultIcon => new (AssetLoader.Open(
        new Uri(Path.Combine(AssetsPath,  DefaultIconName + ".png"))));

    /// <summary>
    /// Load Icon by uri
    /// </summary>
    /// <param name="resourceUri">Uri Path for icon</param>
    /// <returns>Image as <see cref="Bitmap"/></returns>
    public static Bitmap LoadFromResource(Uri resourceUri)
    {
        Bitmap icon;
        try
        {
            icon = new Bitmap(AssetLoader.Open(resourceUri));
        }
        catch (Exception)
        {
            Console.WriteLine("Icon for {0} not found...", resourceUri.AbsolutePath.Split('/').Last());
            icon = DefaultIcon;
        }

        return icon;
    }
    
    /// <summary>
    /// Load Icon by path
    /// </summary>
    /// <param name="resourcePath">String Path for icon</param>
    /// <returns>Image as <see cref="Bitmap"/></returns>
    public static Bitmap LoadFromResource(string resourcePath) =>
        LoadFromResource(new Uri(resourcePath));

    /// <summary>
    /// Set the folder with custom assets
    /// </summary>
    /// <param name="iconsUri">Uri for the folder</param>
    /// <param name="defaultImageName">Name of icon from folder for default image</param>
    /// <remarks><see cref="defaultImageName"/> should be without extension, but file has to be .png</remarks>
    public static void SetAssets(Uri iconsUri, string defaultImageName)
    {
        AssetsPath = iconsUri.OriginalString;
        DefaultIconName = defaultImageName;
    }
}