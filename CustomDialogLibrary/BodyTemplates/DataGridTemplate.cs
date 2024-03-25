using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public class DataGridTemplate : BodyTemplate
{
    public override Control Build(object? param)
    {
        var vm = param as ContentBodyViewModel;
        var grid = new DataGrid
        {
            HeadersVisibility = DataGridHeadersVisibility.All,
            SelectionMode = DataGridSelectionMode.Single,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            [!DataGrid.ItemsSourceProperty] = new Binding(nameof(vm.OuterCollection)),
            [!Layoutable.MaxWidthProperty] = new Binding("$parent.Bounds.Width"),
            [!DataGrid.SelectedItemProperty] = new Binding(nameof(vm.SelectedFileEntity)),
            Columns =
            {
                new DataGridTextColumn
                {
                    Header = "Name",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Name")
                },
                new DataGridTextColumn
                {
                    Header = "Type",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Type")
                },
                new DataGridTextColumn
                {
                    Header = "Created",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("CreationTime")
                },
                new DataGridTextColumn
                {
                    Header = "Modified",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("LastAccessTime")
                },
                new DataGridTextColumn
                {
                    Header = "Size",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Size")
                }
            }
        };

        return grid;
    }

    public override bool Match(object? data) => data is ContentBodyViewModel;
}