namespace CustomDialog.Models.Nodes;

public class ClickableNode : INode, ILoadable, IImagable
{
    public string FullPath { get; }
    public string Title { get; }
    public string IconName => Title;
    public bool Selectable => true;

    public ClickableNode(string path, string title)
    {
        Title = title;
        FullPath = path;
    }
}