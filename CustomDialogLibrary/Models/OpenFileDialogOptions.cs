namespace CustomDialogLibrary.Models;

public record OpenFileDialogOptions() : FileDialogOptionsBase
{
    public bool AllowMultiple { get; init; }
}