using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using CustomDialogLibrary.Interfaces;

namespace DemoApp.Templated_Controls;

public class HeaderButton : TemplatedControl, IImagable
{
    public string IconName { get; }
    
    public static readonly StyledProperty<IImage?> IconProperty = AvaloniaProperty.Register<HeaderButton, IImage?>(
        nameof(Icon));

    public IImage? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<HeaderButton, ICommand>(
            nameof(Command));
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<HeaderButton, object>(
            nameof(CommandParameter),
            defaultValue: null!);
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}