using ReactiveUI;

namespace CustomDialogLibrary.History;

/// <summary>
/// History of directories openings
/// </summary>
public sealed class DirectoryHistory : HistoryBase
{
    #region Static Members
    
    /// <summary>
    /// Gets Default/Home page (node)
    /// </summary>
    public static DirectoryHistory DefaultPage => 
        new ("/");
    
    #endregion
    
    #region Private Fields
    
    private HistoryNode _current;
    
    #endregion
    
    #region Properties
    
    public override IObservable<bool> CanMoveBack { get; }
    public override IObservable<bool> CanMoveForward { get; }
    public override HistoryNode Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }

    #endregion

    private DirectoryHistory(string directoryPath)
    {
        _current = new HistoryNode(directoryPath);
        
        // Whether previous node IS NOT null
        CanMoveBack = this.WhenAnyValue(x => x.Current.PreviousNode,
            selector: prevNode => prevNode is not null);
        
        // Whether next node IS NOT null
        CanMoveForward = this.WhenAnyValue(x => x.Current.NextNode,
            selector: nextNode => nextNode is not null);
    }

    #region Public Methods

    public override void MoveBack() => Current = Current.PreviousNode!;
    public override void MoveForward() => Current = Current.NextNode!;
    public override void Add(string filePath)
    {
        // Created new node (page)
        var node = new HistoryNode(filePath);

        // If new node is not the same as current one then it has to be added
        if (!Current.Equals(node))
        {
            Current.NextNode = node;
            node.PreviousNode = Current;
        }
        
        // Move forward
        Current = node;
    }

    #endregion
}