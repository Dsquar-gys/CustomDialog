using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CustomDialogLibrary.Entities;
using CustomDialogLibrary.Interfaces;
using DynamicData;
using DynamicData.Binding;

namespace CustomDialogLibrary.BodyTemplates;

public abstract class BodyTemplate : IDataTemplate
{
    protected ReadOnlyObservableCollection<FileEntityModel> Collection;

    public void LinkCollection(IBody parent)
    {
        parent.DirectoryData.Connect()
            // Filtering proper extensions
            .Filter(x => parent.Filter.Extensions.Contains(x.Extension) ||
                         string.IsNullOrWhiteSpace(x.Extension) ||
                         parent.Filter.Extensions is [""])
            // Sorting folders first
            .Sort(SortExpressionComparer<FileEntityModel>.Ascending(x => x.GetType().ToString()))
            // Binding to outer collection
            .Bind(out Collection)
            .Subscribe();
    }
    
    public abstract Control Build(object? param);

    public abstract bool Match(object? data);
}