using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CustomDialog.Converters;

public class FilterConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is FileDialogFilter filter)
        {
            if (targetType.IsAssignableTo(typeof(string)))
                return filter.Name ?? "Unnamed filter";
        }
        else if (value is IEnumerable<FileDialogFilter> list)
        {
            if (targetType.IsAssignableTo(typeof(IEnumerable)))
                return list.Select(x => x.Name).ToList();
        }
        else return new BindingNotification(new InvalidCastException("Current type is not implemented yet..."),
                BindingErrorType.Error);
        
        return new BindingNotification(new InvalidCastException("Unknown value type..."),
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}