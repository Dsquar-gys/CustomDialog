using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using CustomDialog.Converters;
using CustomDialog.Models.Entities;
using CustomDialog.ViewModels;

namespace CustomDialog.Views.BodyTemplates;

public class WrapPanelTemplate : BodyTemplate
{
    public override Control Build(object? param)
    {
        BodyViewModel? vm;
        var wpanel = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new ListBox
            {
                [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(vm.DirectoryContent)),
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
                                    Converter = new ImagableConverter()
                                },
                                Width = 75,
                            },
                            new TextBlock
                            {
                                [!TextBlock.TextProperty] = new Binding("Title"),
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

        return wpanel;
    }

    public override bool Match(object? data) => data is BodyViewModel;
}