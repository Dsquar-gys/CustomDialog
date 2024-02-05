using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using CustomDialog.Models;
using CustomDialog.Models.Entities;
using CustomDialog.ViewModels;

namespace CustomDialog.Views.DataTemplates;

public class TemplateSelector : IDataTemplate
{
    public Control? Build(object? param)
    {
        var vm = param as BodyViewModel;
        return vm.Port.LocalDataTemplate.Build(vm);
        
        /*if (param is BodyViewModel vm)
            if (file.Svm.LocalDataTemplate is not null)
                return file.Svm.LocalDataTemplate.Build(file);
        return new TextBlock{Text = "Build error"};*/
    }

    public bool Match(object? data)
    {
        return data is BodyViewModel;
    }
}