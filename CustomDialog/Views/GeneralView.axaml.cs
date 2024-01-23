using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CustomDialog.Models.Nodes;

namespace CustomDialog.Views;

public partial class GeneralView : UserControl
{
    public GeneralView()
    {
        InitializeComponent();
    }

    private void TreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not TreeView tree) return;
        if (tree.SelectedItem is not INode node) return;
        
        tree.SelectedItem = node.Selectable ? node : null;
        if (node is ClickableNode cn)
            LoadViewAsync(cn).ContinueWith(x =>
            {
                Console.WriteLine("Continuation in thread {0}", Environment.CurrentManagedThreadId);
                        
                //MainBody.CustomTextBlock.Text = x.Result;
                PathFinder.Text = x.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private static async Task<string> LoadViewAsync(ClickableNode node) =>
        await Task.Run(async () =>
        {
            Console.WriteLine("Async command in thread {0}", Environment.CurrentManagedThreadId);

            await Task.Delay(1000);
            return node.DirectoryPath;
        });
}