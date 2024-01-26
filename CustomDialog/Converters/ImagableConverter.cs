using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CustomDialog.Models;

namespace CustomDialog.Converters;

public class ImagableConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IImagable imagable)
        {
            string imageURI = "avares://CustomDialog/Assets/Icons";

            if (targetType.IsAssignableTo(typeof(IImage)))
            {
                imageURI = Path.Combine(imageURI, imagable.IconName.ToLower() + ".png");

                return new Bitmap(AssetLoader.Open(new Uri(imageURI)));
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}