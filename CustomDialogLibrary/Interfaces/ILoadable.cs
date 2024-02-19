namespace CustomDialogLibrary.Interfaces;

/// <summary>
/// Entity that can be displayed
/// </summary>
public interface ILoadable
{
    /// <summary>
    /// Gets full path in system
    /// </summary>
    public string FullPath { get; }
    
    /// <summary>
    /// Gets short name (title)
    /// </summary>
    public string Title { get; }
}