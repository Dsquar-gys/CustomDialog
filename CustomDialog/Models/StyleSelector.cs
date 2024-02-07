using CustomDialog.Models.Interfaces;
using CustomDialog.Views.BodyTemplates;

namespace CustomDialog.Models;

public class StyleSelector(BodyTemplate bodyTemplate, string iconName) : IImagable
{
    public string IconName { get; } = iconName;
    public BodyTemplate StyleTemplate { get; } = bodyTemplate;
}