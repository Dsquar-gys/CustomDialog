using System.Collections.ObjectModel;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using DynamicData;

namespace CustomDialogLibrary.Interfaces;

public interface IBody
{
    ReadOnlyObservableCollection<FileEntityModel> OuterDirectoryContent { get; }
    SourceCache<FileEntityModel, string> DirectoryData { get; }
    FileEntityModel? SelectedFileEntity { get; set; }
    BodyTemplate? CurrentStyle { get; set; }
    FileDialogFilter Filter { get; set; }
}