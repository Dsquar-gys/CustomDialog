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
        new (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    
    #endregion
    
    #region Private Fields
    
    private DirectoryNode _current;
    
    #endregion
    
    #region Properties
    
    public override IObservable<bool> CanMoveBack { get; }
    public override IObservable<bool> CanMoveForward { get; }
    public override DirectoryNode Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }

    #endregion

    private DirectoryHistory(string directoryPath)
    {
        var head = new DirectoryNode(directoryPath);
        Current = head;
        
        // Whether previous node IS NOT null
        CanMoveBack = this.WhenAnyValue(x => x.Current.PreviousNode,
            prevNode => !DirectoryNode.IsNull(prevNode));
        
        // Whether next node IS NOT null
        CanMoveForward = this.WhenAnyValue(x => x.Current.NextNode,
            nextNode => !DirectoryNode.IsNull(nextNode));
    }

    #region Public Methods

    public override void MoveBack() => Current = Current.PreviousNode!;
    public override void MoveForward() => Current = Current.NextNode!;
    public override void Add(string filePath)
    {
        // Created new node (page)
        var node = new DirectoryNode(filePath);

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