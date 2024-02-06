using System.IO;
using System.Windows.Input;
using Avalonia.Controls.Templates;
using CustomDialog.Models;
using CustomDialog.ViewModels.Entities;

namespace CustomDialog.ViewModels;

public class SpecificFileViewModel(IDataTemplate template, ICommand command, string iconName) : ISpecificFileViewModel
{
    public IDataTemplate? LocalDataTemplate { get; } = template;
    public ICommand Command { get; } = command;
    public string IconName { get; } = iconName;

    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm)
    {
        vm = null;
        
        if (LocalDataTemplate is null)
            return false;

        if (file is FileInfo fileInfo)
            vm = new FileModel(fileInfo);
        
        if(file is DirectoryInfo directoryInfo)
            vm = new DirectoryModel(directoryInfo);
        return true;
    }
}