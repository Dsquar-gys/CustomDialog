using ReactiveUI;

namespace CustomDialogLibrary.History;

/// <summary>
/// History of directories openings
/// </summary>
public sealed class DirectoryHistory : ReactiveObject
{
    private DirectoryHistoryNode _current;

    public DirectoryHistory(string directoryPath)
    {
        _current = new DirectoryHistoryNode(directoryPath);
        
        // Whether previous node IS NOT null
        CanMoveBack = this.WhenAnyValue(x => x.Current.PreviousNode,
            selector: prevNode => prevNode is not null);
        
        // Whether next node IS NOT null
        CanMoveForward = this.WhenAnyValue(x => x.Current.NextNode,
            selector: nextNode => nextNode is not null);
    }
    
    public IObservable<bool> CanMoveBack { get; }
    public IObservable<bool> CanMoveForward { get; }
    public DirectoryHistoryNode Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }
    
    public void MoveBack() => Current = Current.PreviousNode!;
    public void MoveForward() => Current = Current.NextNode!;
    public void Add(string filePath)
    {
        // Created new node (page)
        var node = new DirectoryHistoryNode(filePath);

        // If new node is not the same as current one then it has to be added
        if (!Current.Equals(node))
        {
            Current.NextNode = node;
            node.PreviousNode = Current;
        }
        
        // Move forward
        Current = node;
    }
}