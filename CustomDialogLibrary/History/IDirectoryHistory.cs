using ReactiveUI;

namespace CustomDialogLibrary.History;

public interface IDirectoryHistory
{
    IObservable<bool> CanMoveBack { get; }
    IObservable<bool> CanMoveForward { get; }
    DirectoryNode Current { get; }
    void MoveBack();
    void MoveForward();
    void Add(string filePath, string name);
}