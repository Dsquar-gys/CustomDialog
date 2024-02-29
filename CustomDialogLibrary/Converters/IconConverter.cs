using System.Globalization;
using Avalonia.Controls;
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
            FileModel file => Resources.Images.ContainsKey(file.Extension.Replace(".", "") + ".png") ?
                file.Extension.Replace(".", "") + ".png" : "file.png",
            DirectoryModel => "folder.png",
            ClickableNode node => node.Title.ToLower() + ".png",
            WrapPanelTemplate => "plates.png",
            DataGridTemplate => "grid.png",
            Button button => button.Tag is not null ? button.Tag.ToString()! : "unknown.png",
            _ => "unknown.png"
        };
        
        if (targetType.IsAssignableTo(typeof(IImage))) return Resources.Images[localIconPath];
        
        throw new NotImplementedException("Unrealized target type");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
}