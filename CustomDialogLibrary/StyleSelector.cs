using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;

namespace CustomDialogLibrary;

/// <summary>
/// Selector for <see cref="StyleBox"/> to contain <see cref="BodyTemplate"/> for <see cref="IBody"/>
/// </summary>
/// <param name="bodyTemplate"><see cref="BodyTemplate"/></param>
/// <param name="iconName">Name of image for icon in assets folder</param>
public class StyleSelector(BodyTemplate bodyTemplate)
{
    /// <summary>
    /// Template for <see cref="IBody"/>
    /// </summary>
    public BodyTemplate StyleTemplate { get; } = bodyTemplate;
}