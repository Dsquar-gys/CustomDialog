using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CustomDialog.Templated_Controls;

public class HeaderButton : Button
{
    public static readonly StyledProperty<IImage?> IconProperty = AvaloniaProperty.Register<HeaderButton, IImage?>(
        nameof(Icon));

    public IImage? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}