using Avalonia.ReactiveUI;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.BodyTemplates;

public partial class FileGridView : ReactiveUserControl<FolderListingVM>
{
    public FileGridView()
    {
        InitializeComponent();
    }
}