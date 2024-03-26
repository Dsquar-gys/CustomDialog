using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

namespace CustomDialogLibrary.BodyTemplates;

public abstract class BodyTemplate : ReactiveObject, IDataTemplate
{
    private bool _allowMultiple;

    public bool AllowMultiple
    {
        get => _allowMultiple;
        set => this.RaiseAndSetIfChanged(ref _allowMultiple, value);
    }
    public abstract Control Build(object? param);
    public abstract bool Match(object? data);
}