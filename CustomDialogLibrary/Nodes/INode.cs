namespace CustomDialogLibrary.Nodes;

public interface INode
{
    public string Title { get; }
    public bool Selectable { get; }
}