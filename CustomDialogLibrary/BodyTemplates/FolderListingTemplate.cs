using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

namespace CustomDialogLibrary.BodyTemplates;

public abstract class FolderListingTemplate : ReactiveObject, IDataTemplate
{
    public abstract string IconName { get; } 
    public abstract Control Build(object? param);
    public abstract bool Match(object? data);
}