using Avalonia.Controls;

namespace CustomDialogLibrary.Models;

public abstract record FileDialogOptionsBase
{
    public string Caption { get; init; } = string.Empty;
    public string InitialDirectory { get; init; } = string.Empty;
    public string InitialFileName { get; init; } = string.Empty;
    public FileDialogFilter[] Filters { get; init; } = [new(){ Name = "All Files", Extensions = [ "*" ]}];
}