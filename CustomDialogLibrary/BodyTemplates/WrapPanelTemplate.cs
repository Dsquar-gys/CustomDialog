using Avalonia.Controls;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public class WrapPanelTemplate: FolderListingTemplate
{
    public override string IconName => "PanelView";

    public override Control Build( object? param )
    {
        var vm = param as FolderListingVM;

        var rv = new IconSetView
        {
            ViewModel = vm,
            ItemList =
            {
                SelectionMode = vm!.AllowMultipleSelection ? SelectionMode.Multiple : SelectionMode.Single
            }
        };

        rv.ItemList.SelectionChanged += ( _, _ ) =>
        {
            vm.SelectedEntities = rv.ItemList
                .SelectedItems!
                .OfType<FileEntityModelBase>()
                .ToList();
        };
        return rv;
    }

    public override bool Match( object? data )
    {
        return data is FolderListingVM;
    }
}