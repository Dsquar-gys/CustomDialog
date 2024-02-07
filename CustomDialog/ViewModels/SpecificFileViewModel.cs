using System.IO;
using System.Windows.Input;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using CustomDialog.Models.Entities;
using CustomDialog.Models.Interfaces;
using CustomDialog.Views.BodyTemplates;

namespace CustomDialog.ViewModels;

public class SpecificFileViewModel(BodyTemplate? template, ICommand? command, string iconName) : ISpecificFileViewModel
{
    public IDataTemplate? LocalDataTemplate { get; } = template ?? new EmptyTemplate();
    public ICommand? Command { get; } = command;
    public string IconName { get; } = iconName;

    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm)
    {
        vm = null!;
        
        if (LocalDataTemplate is EmptyTemplate)
            return false;

        if (file is FileInfo fileInfo)
            vm = new FileModel(fileInfo);
        
        if(file is DirectoryInfo directoryInfo)
            vm = new DirectoryModel(directoryInfo);
        return true;
    }
}