using System.Collections;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CustomDialogLibrary.Converters;

public class FilterConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case FileDialogFilter filter:
            {
                return filter.Name ?? "Unnamed filter";
            }
            case IEnumerable<FileDialogFilter> list:
            {
                if (targetType.IsAssignableTo(typeof(IEnumerable)))
                    return list.Select(x => x.Name).ToList();
                break;
            }
            default:
                return new BindingNotification(new InvalidCastException("Current type is not implemented yet..."),
                    BindingErrorType.Error);
        }

        return new BindingNotification(new InvalidCastException("Unknown value type..."),
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}