using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CustomDialog.Models;

namespace CustomDialog.Views;

public partial class GeneralView : UserControl
{
    public GeneralView()
    {
        InitializeComponent();
    }

    private void TreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TreeView tree)
        {
            if (tree.SelectedItem is INode node)
            {
                tree.SelectedItem = node.Selectable ? node : null;
            }
        }
    }
}