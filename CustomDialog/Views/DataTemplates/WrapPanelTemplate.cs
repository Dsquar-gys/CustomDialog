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

namespace CustomDialog.Views.DataTemplates;

public class WrapPanelTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        var vm = param as BodyViewModel;
        var wpanel = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new ListBox
            {
                [!ListBox.ItemsSourceProperty] = new Binding(nameof(vm.DirectoryContent)),
                [!ListBox.SelectedItemProperty] = new Binding(nameof(vm.SelectedFileEntity)),
                ItemTemplate = new FuncDataTemplate<FileEntityModel>((value, namescope) =>
                    new StackPanel
                    {
                        Children =
                        {
                            new Image
                            {
                                [!Image.SourceProperty] = new Binding
                                {
                                    Path = nameof(value.Svm),
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
                    [!WrapPanel.MaxWidthProperty] = new Binding("$parent[4].Bounds.Width")
                }),
                Styles =
                {
                    new Style(selector => selector.OfType<ListBoxItem>())
                    {
                        Setters =
                        {
                            new Setter(ListBoxItem.WidthProperty, 150d),
                            new Setter(ListBoxItem.HorizontalAlignmentProperty, HorizontalAlignment.Left),
                            new Setter(ListBoxItem.MarginProperty, new Thickness(5d)),
                            new Setter(ListBoxItem.BackgroundProperty, Brushes.Transparent)
                        }
                    }
                }
            }
        };

        return wpanel;
    }

    public bool Match(object? data)
    {
        return data is BodyViewModel;
    }
}