using ReactiveUI;

namespace CustomDialogLibrary.History;

/// <summary>
/// Abstract class for directories openings history
/// </summary>
public abstract class HistoryBase : ReactiveObject
{
    /// <summary>
    /// Observes on ability to move history back
    /// </summary>
    public abstract IObservable<bool> CanMoveBack { get; }
    
    /// <summary>
    /// Observes on ability to move history forward
    /// </summary>
    public abstract IObservable<bool> CanMoveForward { get; }
    
    /// <summary>
    /// Gets current history page (node)
    /// </summary>
    public abstract HistoryNode Current { get; set; }
    
    /// <summary>
    /// Sets current history page to the previous one
    /// </summary>
    public abstract void MoveBack();
    
    /// <summary>
    /// Sets current history page to the next one
    /// </summary>
    public abstract void MoveForward();
    
    /// <summary>
    /// Adds page to history
    /// </summary>
    /// <param name="filePath">Full path of the directory</param>
    public abstract void Add(string filePath);
}