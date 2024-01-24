using System;
using System.Collections.ObjectModel;
using CustomDialog.Models.Nodes;
using CustomDialog.ViewModels.Commands;
using CustomDialog.ViewModels.Entities;
using CustomDialog.ViewModels.History;

namespace CustomDialog.ViewModels;

public class GeneralViewModel : ViewModelBase
{
    public BodyViewModel DVM { get; set; } = new();
    public ObservableCollection<Node> Nodes{ get; }

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