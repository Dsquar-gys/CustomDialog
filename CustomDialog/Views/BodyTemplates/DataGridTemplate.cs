using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using CustomDialog.ViewModels;

namespace CustomDialog.Views.BodyTemplates;

public class DataGridTemplate : BodyTemplate
{
    public override Control Build(object? param)
    {
        var vm = param as BodyViewModel;
        var grid = new DataGrid
        {
            HeadersVisibility = DataGridHeadersVisibility.All,
            SelectionMode = DataGridSelectionMode.Single,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            [!Layoutable.MaxWidthProperty] = new Binding("$parent.Bounds.Width"),
            [!DataGrid.ItemsSourceProperty] = new Binding(nameof(vm.DirectoryContent)),
            [!DataGrid.SelectedItemProperty] = new Binding(nameof(vm.SelectedFileEntity)),
            Columns =
            {
                new DataGridTextColumn
                {
                    Header = "Name",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Title")
                },
                new DataGridTextColumn
                {
                    Header = "Type",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("IconName")
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
                    Header = "Type",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Size")
                }
            }
        };

        return grid;
    }

    public override bool Match(object? data) => data is BodyViewModel;
}