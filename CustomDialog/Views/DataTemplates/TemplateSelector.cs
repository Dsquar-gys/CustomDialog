using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using CustomDialog.Models;
using CustomDialog.Models.Entities;

namespace CustomDialog.Views.DataTemplates;

public class TemplateSelector : IDataTemplate
{
    public Control? Build(object? param)
    {
        //var file = param as FileEntityModel;
        //return file.Svm.LocalDataTemplate.Build(file);
        
        if (param is FileEntityModel file)
            if (file.Svm.LocalDataTemplate is not null)
                return file.Svm.LocalDataTemplate.Build(file);
        return new TextBlock{Text = "Build error"};
    }

    public bool Match(object? data)
    {
        return data is FileEntityModel;
    }
}