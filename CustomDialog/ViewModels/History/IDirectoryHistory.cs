using System;
using System.Collections.Generic;

namespace CustomDialog.ViewModels.History;

public interface IDirectoryHistory : IEnumerable<DirectoryNode>
{
    bool CanMoveBack { get; }

    bool CanMoveForward { get; }

    DirectoryNode Current { get; }

    event EventHandler HistoryChanged;

    void MoveBack();

    void MoveForward();

    void Add(string filePath, string name);
}