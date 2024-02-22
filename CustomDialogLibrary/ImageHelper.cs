using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialogLibrary;

public static class ImageHelper
{
    private static readonly Uri BakedAssetsUri = new("avares://CustomDialogLibrary/BakedAssets");
    private const string BakedDefaultIconName = "unknown.png";

    /// <summary>
    /// Name of default image name
    /// </summary>
    public static string DefaultIconName { get; private set; } = BakedDefaultIconName;

    /// <summary>
    /// Path for assets
    /// </summary>
    public static Uri AssetsUri { get; private set; } = BakedAssetsUri;

    /// <summary>
    /// Gets default image for icon
    /// </summary>
    public static Bitmap DefaultIcon => new (AssetLoader.Open(
        new Uri(Path.Combine(AssetsUri.OriginalString,  DefaultIconName))));
    
    /// <summary>
    /// Contains names of icons for files with specific extensions
    /// </summary>
    public static HashSet<string> AvailableExtensions { get; private set; }

    static ImageHelper()
    {
        UpdateAvailableExtensions();
    }
    
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
    /// <remarks>
    /// <list type="bullet">
    /// <item><description><see cref="defaultImageName"/> has to be with extension .png</description></item>
    /// <item><description><see cref="iconsUri"/> should be in format "avares://*Project*/...", so *Project*.csproj has to contain AvaloniaResource</description></item>
    /// </list>
    /// </remarks>
    public static void SetAssets(Uri iconsUri, string defaultImageName)
    {
        AssetsUri = iconsUri;
        DefaultIconName = defaultImageName;
        try
        {
            UpdateAvailableExtensions();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            AssetsUri = BakedAssetsUri;
            DefaultIconName = BakedDefaultIconName;
        }
    }

    /// <summary>
    /// Overwrites <see cref="AvailableExtensions"/> from Assets folder
    /// </summary>
    private static void UpdateAvailableExtensions()
    {
        var bakedPath = "../../../../" + AssetsUri.Host + AssetsUri.AbsolutePath + "/Extensions";
        var directory = new DirectoryInfo(bakedPath);
        AvailableExtensions = directory.EnumerateFiles()
            .Select(x => '.' + x.Name.Replace(".png", ""))
            .ToHashSet();
    }
}