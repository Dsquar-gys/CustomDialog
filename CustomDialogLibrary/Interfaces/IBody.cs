using System.Reactive;
using Avalonia.Controls;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Entities;
using DynamicData;
using ReactiveUI;

namespace CustomDialogLibrary.Interfaces;

/// <summary>
/// Intended to implement data source 
/// </summary>
public interface IBody
{
    /// <summary>
    /// Data source from current directory
    /// </summary>
    SourceCache<FileEntityModel, string> DirectoryData { get; }
    
    /// <summary>
    /// Selected object
    /// </summary>
    FileEntityModel? SelectedFileEntity { get; set; }
    
    /// <summary>
    /// Gets or sets display style for <see cref="IBody"/>
    /// </summary>
    BodyTemplate? CurrentStyle { get; set; }
    
    /// <summary>
    /// Gets or sets current <see cref="FileDialogFilter"/>
    /// </summary>
    FileDialogFilter Filter { get; set; }
    
    /// <summary>
    /// Gets or sets current directory path
    /// </summary>
    public string? FilePath { get; set; }
    
    ReactiveCommand<FileDialogFilter, Unit> ChangeFilterReactiveCommand { get; }
    ReactiveCommand<object, Unit> ChangeSelectedReactiveCommand { get; }
    ReactiveCommand<object?, Unit> OpenReactiveCommand { get; }
}