using System;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CustomDialog.Models;
using CustomDialog.Models.Interfaces;

namespace CustomDialog.Converters;

public class ImagableConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IImagable imagable) return ImageHelper.DefaultIcon;
        var imageUriPath = "avares://CustomDialog/Assets/Icons";

        if (!targetType.IsAssignableTo(typeof(IImage))) return ImageHelper.DefaultIcon;
        imageUriPath = Path.Combine(imageUriPath, imagable.IconName.ToLower() + ".png");

        return ImageHelper.LoadFromResource(imageUriPath);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}