using Avalonia.Controls;
using CustomDialog.ViewModels;

namespace CustomDialog.Views.BodyTemplates;

public class EmptyTemplate : BodyTemplate
{
    public override Control Build(object? param)
    {
        return new TextBlock
        {
            Text = "Empty Template..."
        };
    }

    public override bool Match(object? data) => data is BodyViewModel;
}