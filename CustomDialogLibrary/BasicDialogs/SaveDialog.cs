using ReactiveUI;

namespace CustomDialogLibrary.BasicDialogs;

public class SaveDialog : ReactiveObject
{
    private string? _directory;
    private string? _initialFileName;
    private string? _defaultExtension;

    public string? Directory
    {
        get => _directory;
        set => this.RaiseAndSetIfChanged(ref _directory, value);
    }
}