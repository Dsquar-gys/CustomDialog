using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;
using DynamicData;

namespace CustomDialogLibrary.BodyTemplates;

public class DataGridTemplate : FolderListingTemplate
{
    public override string IconName { get; } = "ListIcon";

    public override Control Build(object? param)
    {
        var vm = param as FolderListingVM;

        var gridView = new FileGridView()
        {
            ViewModel = vm
        };

        gridView.DataGrid.SelectionMode = DataGridSelectionMode.Single;

        gridView.DataGrid.SelectionChanged += ( sender,
            args ) =>
        {
            vm.SelectedEntities = gridView.DataGrid
                .SelectedItems
                .OfType<FileEntityModelBase>()
                .ToList();
        };
        return gridView;
    }

    public override bool Match(object? data) => data is FolderListingVM;
}