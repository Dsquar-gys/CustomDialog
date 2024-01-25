using System;
using System.Collections.ObjectModel;
using CustomDialog.Models.Nodes;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.Entities;
using CustomDialog.ViewModels.History;
using ReactiveUI;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    private INode _selectedNode;
    
    public BodyViewModel DVM { get; set; } = new();
    public ObservableCollection<Node> Nodes{ get; }

    public INode SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    public GeneralViewModel()
    {
        // Nodes initialization
        Nodes = new ObservableCollection<Node>
        {
            new Node("Places", [
                new ClickableNode("Home"),
                new ClickableNode("Desktop"),
                new ClickableNode("Download")
            ]),
            new Node("FloPPaSS", [
                new ClickableNode("FloPPa1"),
                new ClickableNode("FloPPa2")
            ])
        };
    }
}