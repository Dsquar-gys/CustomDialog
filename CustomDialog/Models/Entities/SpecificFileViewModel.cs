using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls.Templates;

namespace CustomDialog.Models.Entities;

public class SpecificFileViewModel(ICommand? command = null) : ISpecificFileViewModel
{
    private IDataTemplate _localDataTemplate = null;
    public static IDataTemplate? CommonTemplate { get; set; }
    public IDataTemplate? LocalDataTemplate => CommonTemplate;
    public string IconName { get; private set; }
    public string Text { get; private set; }
    public ICommand? Command { get; } = command;
    public FileSystemInfo FileInfo { get; private set; }
    public string? Size { get; private set; }

    public bool TryToCreateFileEntry(FileSystemInfo? file, out FileEntityModel vm)
    {
        vm = null;
        
        if (LocalDataTemplate is null)
            return false;

        if (file is FileInfo info)
        {
            vm = new FileModel(this, info);
            Size = info.Length + " bytes";
            IconName = "file";
        }
        else
        {
            vm = new DirectoryModel(this, file as DirectoryInfo);
            Size = null;
            IconName = "folder";
        }
        
        FileInfo = file;
        Text = vm.Title;
        
        return true;
    }
}