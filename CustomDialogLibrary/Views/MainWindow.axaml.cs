using Avalonia.Controls;
using Avalonia.Interactivity;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.ViewModels;
using ReactiveUI;

namespace CustomDialogLibrary.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? _, RoutedEventArgs e)
    {
        var content = this.GeneralControl.Content as GeneralViewModel;

        content.WhenAnyValue(x => x.ToClose)
            .Subscribe(toClose =>
            {
                if (!toClose) return;
                var entity = content.BodyVM.SelectedFileEntity;
                var result = entity switch
                {
                    FileModel => new FileInfo(entity.FullPath),
                    _ => null
                };
                    
                Close(result);
            });
    }
}