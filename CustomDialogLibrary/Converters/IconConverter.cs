using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CustomDialogLibrary.Converters;

public class IconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var localIconPath = value switch
        {
            "PanelView" => StreamGeometry.Parse("M3,11H11V3H3M3,21H11V13H3M13,21H21V13H13M13,3V11H21V3"),
            "GridView" => StreamGeometry.Parse("M22 20V4C22 2.9 21.1 2 20 2H4C2.9 2 2 2.9 2 4V20C2 21.1 2.9 22 4 22H20C21.1 22 22 21.1 22 20M4 6.5V4H20V6.5H4M4 11V8.5H20V11H4M4 15.5V13H20V15.5H4M4 20V17.5H20V20H4Z"),
            "FileModelIcon" => StreamGeometry.Parse("M13,9V3.5L18.5,9M6,2C4.89,2 4,2.89 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2H6Z"),
            "FolderModelIcon" => StreamGeometry.Parse("M10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6H12L10,4Z"),
            _ => StreamGeometry.Parse("M19.35,10.03C18.67,6.59 15.64,4 12,4C9.11,4 6.6,5.64 5.35,8.03C2.34,8.36 0,10.9 0,14A6,6 0 0,0 6,20H19A5,5 0 0,0 24,15C24,12.36 21.95,10.22 19.35,10.03M13,17H11V15H13V17M14.8,11.82C14.5,12.21 14.13,12.5 13.67,12.75C13.41,12.91 13.24,13.07 13.15,13.26C13.06,13.45 13,13.69 13,14H11C11,13.45 11.11,13.08 11.3,12.82C11.5,12.56 11.85,12.25 12.37,11.91C12.63,11.75 12.84,11.56 13,11.32C13.15,11.09 13.23,10.81 13.23,10.5C13.23,10.18 13.14,9.94 12.96,9.76C12.78,9.56 12.5,9.47 12.2,9.47C11.93,9.47 11.71,9.55 11.5,9.7C11.35,9.85 11.25,10.08 11.25,10.39H9.28C9.23,9.64 9.5,9 10.06,8.59C10.6,8.2 11.31,8 12.2,8C13.14,8 13.89,8.23 14.43,8.68C14.97,9.13 15.24,9.75 15.24,10.5C15.24,11 15.09,11.41 14.8,11.82Z")
        };
        
        return localIconPath;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}