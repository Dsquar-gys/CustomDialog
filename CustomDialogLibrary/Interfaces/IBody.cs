using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using DynamicData;

namespace CustomDialogLibrary.Interfaces;

public interface IBody
{
    SourceCache<FileEntityModel, string> DirectoryData { get; }
    FileEntityModel? SelectedFileEntity { get; set; }
    BodyTemplate? CurrentStyle { get; set; }
    FileDialogFilter Filter { get; }
}