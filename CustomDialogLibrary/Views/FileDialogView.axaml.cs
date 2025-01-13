using Avalonia.ReactiveUI;
using CustomDialogLibrary.ViewModels;

namespace CustomDialogLibrary.Views;

public partial class FileDialogView : ReactiveUserControl<FileDialogVM>
{
    public FileDialogView()
    {
        InitializeComponent();
    }
}