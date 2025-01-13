using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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