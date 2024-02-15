using ReactiveUI;

namespace CustomDialogLibrary.History;

public abstract class HistoryBase : ReactiveObject, IDirectoryHistory
{
    public abstract IObservable<bool> CanMoveBack { get; }
    public abstract IObservable<bool> CanMoveForward { get; }
    public abstract DirectoryNode Current { get; set; }
    public abstract void MoveBack();
    public abstract void MoveForward();
    public abstract void Add(string filePath, string name);
}