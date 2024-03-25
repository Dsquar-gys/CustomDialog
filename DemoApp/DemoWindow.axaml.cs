using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CustomDialogLibrary.BasicDialogs;

namespace DemoApp;

public partial class DemoWindow : Window
{
    public DemoWindow()
    {
        InitializeComponent();
    }

    private async void GetOpenDialog(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenDialog();
        var temp = await dialog.ShowAsync(this);
        
        if (temp?.Length > 0)
            foreach (var str in temp)
                Console.WriteLine(str);
        else Console.WriteLine("No data...");
    }

    private void GetSaveDialog(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}