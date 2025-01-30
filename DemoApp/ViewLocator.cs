using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.ViewModels;
using CustomDialogLibrary.Views;
using DemoApp.ViewModels;
using DemoApp.Views;

namespace DemoApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        // var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        // name = name.Replace("VM", "View", StringComparison.Ordinal);

        var newType = data.GetType().Name switch
        {
            nameof(DemoViewModel) => typeof(DemoWindow),
            nameof(BaseDialogWindowViewModel) => typeof(BaseDialogWindow),
            nameof(FileDialogVM) => typeof(FileDialogView),
            nameof(WrapPanelTemplate) => typeof(IconSetView),
            nameof(DataGridTemplate) => typeof(FileGridView),
            _ => null
        };

        if (newType == null) return new TextBlock { Text = "View Not Found for: " + data.GetType() };
        
        var control = (Control)Activator.CreateInstance(newType)!;
        control.DataContext = data;
        return control;

    }

    public bool Match(object? data) => data is ViewModelBase;
}