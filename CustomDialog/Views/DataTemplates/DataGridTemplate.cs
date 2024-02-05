using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using CustomDialog.ViewModels;

namespace CustomDialog.Views.DataTemplates;

public class DataGridTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        var vm = param as BodyViewModel;
        var grid = new DataGrid
        {
            HeadersVisibility = DataGridHeadersVisibility.All,
            SelectionMode = DataGridSelectionMode.Single,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            [!DataGrid.MaxWidthProperty] = new Binding("$parent.Bounds.Width"),
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
                    Binding = new Binding("Svm.IconName")
                },
                new DataGridTextColumn
                {
                    Header = "Created",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Svm.FileInfo.CreationTime")
                },
                new DataGridTextColumn
                {
                    Header = "Modified",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Svm.FileInfo.LastAccessTime")
                },
                new DataGridTextColumn
                {
                    Header = "Type",
                    Width = new DataGridLength(2d, DataGridLengthUnitType.Star),
                    Binding = new Binding("Svm.Size")
                }
            }
        };

        return grid;
    }

    public bool Match(object? data)
    {
        return data is BodyViewModel;
    }
}