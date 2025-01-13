using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using CustomDialogLibrary.Converters;
using CustomDialogLibrary.Models;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public class WrapPanelTemplate: FolderListingTemplate
{
    public override string IconName { get; } = "SquareGridIcon";

    public override Control Build( object? param )
    {
        var vm = param as FolderListingVM;

        var rv = new IconSetView
        {
            ViewModel = vm
        };

        rv.ItemList.SelectionMode = SelectionMode.Single;
    
        rv.ItemList.SelectionChanged += ( sender,
            args ) =>
        {
            vm.SelectedEntities = rv.ItemList
                .SelectedItems
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