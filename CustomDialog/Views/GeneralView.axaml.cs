using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CustomDialog.Models.Nodes;

namespace CustomDialog.Views;

public partial class GeneralView : UserControl
{
    public GeneralView()
    {
        InitializeComponent();
    }

    private async void TreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TreeView tree)
            if (tree.SelectedItem is INode node)
            {
                tree.SelectedItem = node.Selectable ? node : null;
                if (node is ClickableNode cn)
                    LoadViewAsync(cn);
            }
    }

    private async Task LoadViewAsync(ClickableNode node) =>
        await Task.Run(() =>
        {
            Console.WriteLine("Async command in thread {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            Dispatcher.UIThread.Invoke(() =>
            {
                MainBody.CustomTextBlock.Text = node.DirectoryPath;
                PathFinder.Text = node.DirectoryPath;
            });
        });
}