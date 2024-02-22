using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.SideBarEntities;

namespace CustomDialogLibrary.Converters;

public class IconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var localIconPath = value switch
        {
            FileModel file => ImageHelper.AvailableExtensions.Contains(file.Extension) ?
                $"Extensions/{file.Extension[1..]}.png" : "file.png",
            DirectoryModel => "folder.png",
            ClickableNode node => node.Title.ToLower() + ".png",
            WrapPanelTemplate => "plates.png",
            DataGridTemplate => "grid.png",
            _ => ImageHelper.DefaultIconName
        };
        
        var imageUriPath = Path.Combine(ImageHelper.AssetsUri.OriginalString, localIconPath);

        if (targetType.IsAssignableTo(typeof(IImage))) return ImageHelper.LoadFromResource(imageUriPath);

        throw new NotImplementedException("Unrealized target type");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}