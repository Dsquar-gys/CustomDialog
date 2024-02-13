using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CustomDialogLibrary.Interfaces;

namespace CustomDialogLibrary.Converters;

public class ImagableConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IImagable imagable) return ImageHelper.DefaultIcon;
        var imageUriPath = ImageHelper.AssetsPath;

        if (!targetType.IsAssignableTo(typeof(IImage))) return ImageHelper.DefaultIcon;
        imageUriPath = Path.Combine(imageUriPath, imagable.IconName.ToLower() + ".png");

        return ImageHelper.LoadFromResource(imageUriPath);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}