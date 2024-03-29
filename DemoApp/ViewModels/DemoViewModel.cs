using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.BasicDialogs;
using CustomDialogLibrary.ViewModels;
using ReactiveUI;

namespace DemoApp.ViewModels;

public class DemoViewModel : ViewModelBase
{
    public ReactiveCommand<Window, Unit> GetOpenDialogCommand { get; } = ReactiveCommand.CreateFromTask<Window>(
        async parent =>
        {
            var dialog = new OpenDialog();

            //dialog.Directory = "/home/dmitrichenkoda@kvant-open.spb.ru/RiderProjects/";
            //dialog.AllowMultiple = true;
            //dialog.Filters = new List<FileDialogFilter>();

            var temp = await dialog.ShowAsync(parent);

            if (temp?.Length > 0)
                foreach (var str in temp)
                    Console.WriteLine(str);
            else Console.WriteLine("No data...");
        });

    public ReactiveCommand<Window, Unit> GetSaveDialogCommand { get; } = ReactiveCommand.CreateFromTask<Window>(
        async parent =>
        {
            var dialog = new SaveDialog();

            dialog.InitialFileName = "FloPPa";
            dialog.DefaultExtension = "txt";

            var temp = await dialog.ShowAsync(parent);

            Console.WriteLine(temp ?? "No data...");
        });
}