using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.Interfaces;

namespace CustomDialogLibrary;

public class StyleSelector(BodyTemplate bodyTemplate, string iconName) : IImagable
{
    public string IconName { get; } = iconName;
    public BodyTemplate StyleTemplate { get; } = bodyTemplate;
}