using System;
using Avalonia;
using Avalonia.Controls;

namespace CustomDialog.Models.Interactions;

public class TreeViewItemInteraction : AvaloniaObject
{
    static TreeViewItemInteraction()
    {
        ListenToExpandedProperty.Changed.Subscribe(x => OnListenToExpandedChanged(x.Sender, x.NewValue.GetValueOrDefault<bool>()));
    }
    
    private static void OnListenToExpandedChanged(AvaloniaObject  sender, bool value)
    {
        // if the property was set to true, add the needed event listener
        if (value && sender is TreeViewItem item)
        {
            item.PropertyChanged += TreeViewItemOnPropertyChanged;
        }
    }
    
    private static void TreeViewItemOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // if IsExpanded was changed, perform the needed actions
        if(e.Property == TreeViewItem.IsExpandedProperty)
        {
            try
            {
                (sender as TreeViewItem).IsExpanded = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
    
    // Defines the ListenToExpanded attached property
    public static readonly AttachedProperty<bool> ListenToExpandedProperty =
        AvaloniaProperty.RegisterAttached<TreeViewItemInteraction, TreeViewItem, bool>("ListenToExpanded");
}