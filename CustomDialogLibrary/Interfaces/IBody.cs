using System.Collections.ObjectModel;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;

namespace CustomDialogLibrary.Interfaces;

public interface IBody
{
    ObservableCollection<FileEntityModel> OuterDirectoryContent { get; }
    List<FileEntityModel> FullDirectoryContent { get; }
    FileEntityModel? SelectedFileEntity { get; set; }
    BodyTemplate? CurrentStyle { get; set; }
}