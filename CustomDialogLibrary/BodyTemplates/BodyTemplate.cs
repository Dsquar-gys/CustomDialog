using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace CustomDialogLibrary.BodyTemplates;

public abstract class BodyTemplate : IDataTemplate
{
    public abstract Control Build(object? param);

    public abstract bool Match(object? data);
}