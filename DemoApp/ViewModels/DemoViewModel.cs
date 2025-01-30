using System;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.ViewModels;
using ReactiveUI;

namespace DemoApp.ViewModels;

public class DemoViewModel : ViewModelBase
{
    public ReactiveCommand<Window, Unit> OpenFileDialogCommand { get; } = ReactiveCommand.CreateFromTask<Window>(
        async parent =>
        {
            var dialog = new OpenFileDialog();
            dialog.Pending.Subscribe(Console.WriteLine);

            await dialog.AskUser(parent);
        });

    // public ReactiveCommand<Window, Unit> GetSaveDialogCommand { get; } = ReactiveCommand.CreateFromTask<Window>(
    //     async parent =>
    //     {
    //         var dialog = new SaveDialog();
    //
    //         dialog.InitialFileName = "FloPPa";
    //         dialog.DefaultExtension = "txt";
    //
    //         var temp = await dialog.ShowAsync(parent);
    //
    //         Console.WriteLine(temp ?? "No data...");
    //     });
}