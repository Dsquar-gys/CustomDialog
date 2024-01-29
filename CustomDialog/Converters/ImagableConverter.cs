using System;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CustomDialog.Models;

namespace CustomDialog.Converters;

public class ImagableConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IImagable imagable)
        {
            string imageURIPath = "avares://CustomDialog/Assets/Icons";

            if (targetType.IsAssignableTo(typeof(IImage)))
            {
                imageURIPath = Path.Combine(imageURIPath, imagable.IconName.ToLower() + ".png");

                return ImageHelper.LoadFromResource(imageURIPath);
            }
        }

        return ImageHelper.DefaultIcon;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}