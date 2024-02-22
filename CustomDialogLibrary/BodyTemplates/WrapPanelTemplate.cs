using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using CustomDialogLibrary.Converters;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public class WrapPanelTemplate: BodyTemplate
{
    public override Control Build(object? param)
    {
        var vm = param as BodyViewModel;
        
        var wrapPanel = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new ListBox
            {
                [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(vm.OuterCollection)),
                [!SelectingItemsControl.SelectedItemProperty] = new Binding(nameof(vm.SelectedFileEntity)),
                ItemTemplate = new FuncDataTemplate<FileEntityModel>((value, _) =>
                    new StackPanel
                    {
                        Children =
                        {
                            new Image
                            {
                                [!Image.SourceProperty] = new Binding(".")
                                {
                                    Converter = new IconConverter()
                                },
                                Width = 75,
                            },
                            new TextBlock
                            {
                                [!TextBlock.TextProperty] = new Binding(nameof(value.Title)),
                                HorizontalAlignment = HorizontalAlignment.Center,
                            },
                        },
                    }),
                ItemsPanel = new FuncTemplate<Panel?>(() => new WrapPanel
                {
                    [!Layoutable.MaxWidthProperty] = new Binding("$parent[4].Bounds.Width")
                }),
                Styles =
                {
                    new Style(selector => selector.OfType<ListBoxItem>())
                    {
                        Setters =
                        {
                            new Setter(Layoutable.WidthProperty, 150d),
                            new Setter(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Left),
                            new Setter(Layoutable.MarginProperty, new Thickness(5d)),
                            new Setter(TemplatedControl.BackgroundProperty, Brushes.Transparent)
                        }
                    }
                }
            }
        };

        return wrapPanel;
    }

    public override bool Match(object? data) => data is BodyViewModel;
}