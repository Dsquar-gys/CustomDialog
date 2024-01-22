using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CustomDialog.Templated_Controls;

public class HoverButton : TemplatedControl
{
    public static readonly StyledProperty<IImage?> IconProperty = AvaloniaProperty.Register<HoverButton, IImage?>(
        nameof(Icon));

    public IImage? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}