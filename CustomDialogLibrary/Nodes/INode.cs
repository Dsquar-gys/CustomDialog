namespace CustomDialogLibrary.Nodes;

public interface INode
{
    /// <summary>
    /// Gets name of the Node
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    /// Gets whether node is selectable or not
    /// </summary>
    public bool Selectable { get; }
}