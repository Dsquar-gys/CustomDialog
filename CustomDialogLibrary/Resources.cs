using System.Reflection;
using Avalonia.Media.Imaging;

namespace CustomDialogLibrary;

public static class Resources
{
    public static Dictionary<string, Bitmap> Images { get; }

    static Resources() // !!! EXTENSIONS !!!
    {
        var assembly = Assembly.GetExecutingAssembly();
        var names = assembly.GetManifestResourceNames();
        Images = new();

        foreach (var name in names)
        {
            using var stream = assembly.GetManifestResourceStream(name)!;
            try
            {
                var img = new Bitmap(stream);
                var splitted = name.Split('.');
                
                Images.Add(splitted[^2] + '.' + splitted[^1], img);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Resource is not an image");
            }
        }
    }
}