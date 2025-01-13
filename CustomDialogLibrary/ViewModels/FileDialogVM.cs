using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CustomDialogLibrary.BodyTemplates;
using CustomDialogLibrary.History;
using CustomDialogLibrary.Interfaces;
using CustomDialogLibrary.Models;
using ReactiveUI;

namespace CustomDialogLibrary.ViewModels;

public class FileDialogVM : ViewModelBase, IDisposable
{
    private static readonly FilePickerFileType Filter = new( "Все файлы" ) { Patterns = ["*.*"] };
    
    private readonly DirectoryHistory _history;
    
    private string _editableFolder;
    
    private FilePickerFileType[] _filters =
    [
        new( "Все файлы" ) { Patterns = ["*.*"] },
        new( "Изображения" )
            { Patterns = MediaFormats.ImageExtensions.Select( p => $"*.{p}" ).ToArray() },
        new( "Аудио" ) { Patterns = MediaFormats.AudioExtensions.Select( p => $"*.{p}" ).ToArray() },
        new( "Видео" ) { Patterns = MediaFormats.VideoExtensions.Select( p => $"*.{p}" ).ToArray() }
    ];
    
    private string _folder;
    
    private ClickableNode? _selectedNode;

    public FileDialogVM(string initialDirectory, IDialogCustomizationsFactory? sfvm = null)
    {
        _history = new DirectoryHistory( initialDirectory );
        _folder  = initialDirectory;
        
        OkCmd     = ReactiveCommand.Create( () => {} );
        CancelCmd = ReactiveCommand.Create( () => {} );

        DoubleTappedCmd = ReactiveCommand.Create(OnDoubleTapped);
        MoveBackCommand = ReactiveCommand.Create(OnMoveBack, _history.CanMoveBack);
        MoveForwardCommand = ReactiveCommand.Create(OnMoveForward, _history.CanMoveForward);
        MoveUpCommand = ReactiveCommand.Create(OnMoveUp);
        
        // Sidebar tree nodes init
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Unix:
                SideBarNodes =
                [
                    new ("System", [
                        new ClickableNode("/", "Root")
                    ]),
                    new("Places", [
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Home"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop"),
                        new ClickableNode(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),"Download"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures")
                    ])
                ];
                break;
            case PlatformID.Win32NT:
                var drives = DriveInfo.GetDrives();

                SideBarNodes = 
                [
                    new("System", new ObservableCollection<ClickableNode>( drives.Select( drive =>
                        new ClickableNode( drive.Name,
                            drive
                                .Name ) ) ) ),
                    new("Places", [
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Home"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop"),
                        new ClickableNode(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                            "Download"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            "Documents"),
                        new ClickableNode(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures")
                    ])
                ];
                break;
            default:
                throw new NotSupportedException("This OS platform is not supported");
        }

        // BodyStyleBox init
        DialogCustomizationsCustomizations = sfvm ?? new DefaultFileDialogCustomizationsFactory(
        [
            new WrapPanelTemplate(),
            new DataGridTemplate()
        ]);
        
        // Body creation
        FileList = new FolderListingVM(this.WhenAnyValue(x => x.Folder).DistinctUntilChanged()
            .Where(x => x is not null)
            .Select(x => x))
        {
            DialogCustomizationsFactory = DialogCustomizationsCustomizations
        };
        
        // Style of Body depends on BodyStyleBox.CurrentBodyTemplate
        DialogCustomizationsCustomizations.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(t => { FileList.CurrentStyle = t; });

        this.WhenAnyValue(t => t.SelectedNode)
            .DistinctUntilChanged()
            .Where(x => x is not null)
            .Select(x => x)
            .Select(node => node.Path)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(OpenNewFolder);

        this.WhenAnyValue(t => t.Filters)
            .DistinctUntilChanged()
            .Where(x => x is not null)
            .Select(x => x)
            .Subscribe(types => FileList.Filter = types.FirstOrDefault() ?? Filter);

        this.WhenAnyValue(t => t.EditableFolder)
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromMilliseconds(100))
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Select(CheckUserInputFolder)
            .DistinctUntilChanged()
            .Where(x => x is not null)
            .Select(x => x)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(OpenNewFolder);

        this.WhenAnyValue(t => t.Folder)
            .DistinctUntilChanged()
            .Subscribe(folder =>
            {
                EditableFolder = Folder;

                SelectedNode = SideBarNodes.SelectMany(n => n.SubNodes)
                    .FirstOrDefault(sn => sn.Path == folder);
            });
    }
    
    public ReactiveCommand<Unit, Unit> OkCmd { get; }
    public ReactiveCommand<Unit, Unit> CancelCmd { get; }
    public ReactiveCommand<Unit, Unit> MoveBackCommand    { get; }
    public ReactiveCommand<Unit, Unit> MoveForwardCommand { get; }
    public ReactiveCommand<Unit, Unit> DoubleTappedCmd    { get; }
    public ReactiveCommand<Unit, Unit> MoveUpCommand      { get; }
    
    /// <summary>
    /// Body for content
    /// </summary>
    public FolderListingVM FileList { get; }

    /// <summary>
    /// Gets <see cref="BodyStyleBox"/> for <see cref="FileList"/>
    /// </summary>
    public IDialogCustomizationsFactory DialogCustomizationsCustomizations { get; }
    
    /// <summary>
    /// Gets collection of sidebar tree nodes
    /// </summary>
    public SideBarNode[] SideBarNodes { get; }
    
    /// <summary>
    /// Gets Selected node on sidebar tree
    /// </summary>
    public ClickableNode? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    /// <summary>
    /// Gets collection of filters for content by extensions
    /// </summary>
    public FilePickerFileType[] Filters
    {
        get => _filters;
        set => this.RaiseAndSetIfChanged( ref _filters, value );
    }
    
    public string Folder
    {
        get => _folder;
        set => this.RaiseAndSetIfChanged( ref _folder, value );
    }
    
    public string EditableFolder
    {
        get => _editableFolder;
        set => this.RaiseAndSetIfChanged( ref _editableFolder, value );
    }

    private void OnDoubleTapped()
    {
        if( FileList.SelectedEntities is null )
        {
            CancelCmd.Execute();

            return;
        }

        if( FileList.SelectedEntities.Count == 1 && FileList.SelectedEntities[0] is DirectoryModel dm )
        {
            OpenNewFolder( dm.FullPath );

            return;
        }

        OkCmd.Execute();
    }

    private void OnMoveForward()
    {
        _history.MoveForward();
        Folder = _history.Current.Path;
    }

    private void OnMoveBack()
    {
        _history.MoveBack();
        Folder = _history.Current.Path;
    }

    private void OnMoveUp()
    {
        var path = Folder;
    
        if( string.IsNullOrWhiteSpace( path ) )
        {
            return;
        }

        path = Path.GetDirectoryName( path );
    
        OpenNewFolder( path );
    }
    
    private void OpenNewFolder( string path )
    {
        if( string.IsNullOrWhiteSpace( path ) )
        {
            return;
        }

        if( !Directory.Exists( path ) )
        {
            return;
        }

        Folder = path;
        _history.Add( path );
    }
    
    private string? CheckUserInputFolder( string? arg )
    {
        if( arg == Folder )
        {
            return null;
        }

        if( string.IsNullOrWhiteSpace( arg ) )
        {
            return null;
        }

        if( !Directory.Exists( arg ) )
        {
            return null;
        }

        return arg;
    }
    
    public void Dispose()
    {
        FileList.Dispose();
    }
}