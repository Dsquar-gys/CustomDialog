using System;
using Avalonia.Controls;
using CustomDialogLibrary;

namespace DemoApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        ImageHelper.SetAssets(new Uri("avares://DemoApp/Assets/Icons"), "unknown.png");
        InitializeComponent();
    }
}