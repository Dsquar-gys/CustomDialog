using ReactiveUI;

namespace CustomDialogLibrary.History;

public interface IDirectoryHistory
{
    /// <summary>
    /// Observes on ability to move history back
    /// </summary>
    IObservable<bool> CanMoveBack { get; }
    
    /// <summary>
    /// Observes on ability to move history forward
    /// </summary>
    IObservable<bool> CanMoveForward { get; }
    
    /// <summary>
    /// Gets current history page (node)
    /// </summary>
    DirectoryNode Current { get; }
    
    /// <summary>
    /// Sets current history page to the previous one
    /// </summary>
    void MoveBack();
    
    /// <summary>
    /// Sets current history page to the next one
    /// </summary>
    void MoveForward();
    
    /// <summary>
    /// Adds page to history
    /// </summary>
    /// <param name="filePath">Full path of the directory</param>
    /// <param name="name">Title of directory</param>
    void Add(string filePath);
}