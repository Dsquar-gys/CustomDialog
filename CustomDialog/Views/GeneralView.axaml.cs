using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CustomDialog.Models;
using CustomDialog.Models.Nodes;
using CustomDialog.ViewModels;

namespace CustomDialog.Views;

public partial class GeneralView : UserControl
{
    private CancellationTokenSource tokenSource = new();
    private CancellationToken token;
    private Task CurrentTask = new Task(() => {});
    
    public GeneralView()
    {
        InitializeComponent();
        
        token = tokenSource.Token;
        CurrentTask.Start();
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

    private async Task LoadView(ClickableNode node)
    {
        /*if (!CurrentTask.IsCompleted)
        {
            await tokenSource.CancelAsync();
            
            CurrentTask.ContinueWith(x =>
            {
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
            }, TaskContinuationOptions.OnlyOnCanceled);

            try
            {
                CurrentTask.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }*/
        //}
        
        CurrentTask = Task.Run(() =>
        {
            token.ThrowIfCancellationRequested();
            
            Console.WriteLine("Current thread: {0}", Thread.CurrentThread.ManagedThreadId);
            // Query
            Thread.Sleep(2000);
            
            if (token.IsCancellationRequested)
                Console.WriteLine("Cancelled");
            token.ThrowIfCancellationRequested();
            
            Dispatcher.UIThread.Invoke(() => MainBody.CustomTextBlock.Text = node.DirectoryPath);
        }, token);

        await CurrentTask;
        
        

        /*await Task.Run(() =>
        {
            //token.ThrowIfCancellationRequested();

            Console.WriteLine("Current thread: {0}", Thread.CurrentThread.ManagedThreadId);
            // Query
            Thread.Sleep(1000);


            Dispatcher.UIThread.Invoke(() => MainBody.CustomTextBlock.Text = node.DirectoryPath);
        }, token);*/

    }
}