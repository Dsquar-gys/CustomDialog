using System;
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

public class TableTemplate : IDataTemplate
{
    /*
       <StackPanel Orientation="Horizontal">
           <Image x:DataType="entities:FileEntityModel"
                  Source="{Binding Converter={StaticResource ImagableConverter}}"
                  Width="50"/>
           <TextBlock x:DataType="entities:FileEntityModel"
                      Text="{Binding Title}"
                      HorizontalAlignment="Center"
                      VerticalAlignment="Center"
                      Margin="3,0"/>
       </StackPanel>
       */
    public Control? Build(object? param)
    {
        var sender = param as FileEntityModel;
        var res = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children =
            {
                new Image
                {
                    [!Image.SourceProperty] = new Binding
                    {
                        Path = nameof(sender.Svm),
                        Converter = new ImagableConverter()
                    },
                    Width = 50,
                },
                new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("Title"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(3, 0),
                },
            }
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