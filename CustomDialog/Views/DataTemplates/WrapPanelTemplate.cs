using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using CustomDialog.Converters;
using CustomDialog.Models;
using CustomDialog.Models.Entities;

namespace CustomDialog.Views.DataTemplates;

public class WrapPanelTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        var sender = param as FileEntityModel;
        var res = new StackPanel
        {
            Children =
            {
                new Image
                {
                    [!Image.SourceProperty] = new Binding
                    {
                        Path = nameof(sender.Svm),
                        Converter = new ImagableConverter()
                    },
                    Width = 75,
                },
                new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("Title"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                },
            },
        };

        res.DoubleTapped += (sender, args) =>
        {
            var file = param as FileEntityModel;
            if (file.Svm.Command is not null)
                file.Svm.Command.Execute(file);
        };

        return res;
    }

    public bool Match(object? data)
    {
        return data is FileEntityModel;
    }
}