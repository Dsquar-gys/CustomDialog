using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;

namespace CustomDialogLibrary.Nodes;

/// <summary>
/// Node for sidebar tree
/// </summary>
/// <param name="title">Name of the node</param>
/// <param name="subNodes">Collection of children nodes</param>
public class Node(string title, ObservableCollection<ClickableNode>? subNodes = null)
{
    /// <summary>
    /// Gets collection of children nodes
    /// </summary>
    public ObservableCollection<ClickableNode>? SubNodes { get; } = subNodes;
    public string Title { get; } = title;
}