using Avalonia.Controls;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public class DataGridTemplate : FolderListingTemplate
{
    public override string IconName => "GridView";

    public override Control Build(object? param)
    {
        var vm = param as FolderListingVM;

        var gridView = new FileGridView
        {
            ViewModel = vm,
            DataGrid =
            {
                SelectionMode = vm!.AllowMultipleSelection ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single
            }
        };

        gridView.DataGrid.SelectionChanged += ( _, _ ) =>
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