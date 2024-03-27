using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.ViewModels;
using DynamicData;

namespace CustomDialogLibrary.BodyTemplates;

public class DataGridTemplate : BodyTemplate
{
    public override Control Build(object? param)
    {
        var vm = param as ContentViewModel;
        var grid = new DataGrid
        {
            HeadersVisibility = DataGridHeadersVisibility.All,
            SelectionMode = AllowMultiple switch
            {
                false => DataGridSelectionMode.Single,
                true => DataGridSelectionMode.Extended
            },
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            [!DataGrid.ItemsSourceProperty] = new Binding(nameof(vm.OuterCollection)),
            [!Layoutable.MaxWidthProperty] = new Binding("$parent.Bounds.Width"),
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

        grid.SelectionChanged += (sender, args) =>
        {
            vm.SelectedEntities.Clear();
            vm.SelectedEntities.AddRange(grid.SelectedItems.OfType<FileEntityModel>());
        };
        
        return grid;
    }

    public override bool Match(object? data) => data is ContentViewModel;
}