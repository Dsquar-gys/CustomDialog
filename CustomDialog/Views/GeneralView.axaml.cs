using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CustomDialog.Models.Nodes;
using CustomDialog.Templated_Controls;

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
            if (tree.SelectedItem is INode node)
            {
                tree.SelectedItem = node.Selectable ? node : null;
                if (node is ClickableNode cn)
                    LoadView(cn);
            }
    }

    private async Task LoadView(ClickableNode node) =>
        await Task.Run(() =>
        {
            Console.WriteLine("Async command in thread {0}", Thread.CurrentThread.ManagedThreadId);
            Dispatcher.UIThread.Invoke(() =>
            {
                MainBody.CustomTextBlock.Text = node.DirectoryPath;
                PathFinder.Text = node.DirectoryPath;
            });
        });
}