using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Platform.Storage;
using DocumentCollector.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DocumentCollector.FileSystemIndexer;

public class FileSystemIndexerConfigViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly IContext _ctx;
    private readonly IStorageProvider _storageProvider;
    private FileSystemIndexerConfig _config = new();


    private int _docsToFindCount;
    public int DocsToFindCount
    {
        get => _docsToFindCount;
        set => SetProperty(ref _docsToFindCount, value);
    }

    private List<string> _selectedDirectories = new();

    public List<string> SelectedDirectories
    {
        get => _selectedDirectories;
        set => SetProperty(ref _selectedDirectories, value);
    }
    
    public ObservableCollection<string> Directories { get; } = new();

    private string _editExtensions;

    public string EditExtensions
    {
        get => _editExtensions;
        set => SetProperty(ref _editExtensions, value);
    }

    private string _nonEditExtensions;

    public string NonEditExtensions
    {
        get => _nonEditExtensions;
        set => SetProperty(ref _nonEditExtensions, value);
    }

    private DelegateCommand? _addFolderCmd;
    public DelegateCommand AddFolderCmd => _addFolderCmd ??= new(ExecuteAddFolderCmd);

    private async void ExecuteAddFolderCmd()
    {
        var opts = new FolderPickerOpenOptions()
        {
            Title = "Wybierz folder z listami xlsx",
            AllowMultiple = true,
        };
        var results = await _storageProvider.OpenFolderPickerAsync(opts);
        if (results is null || results.Count == 0)
        {
            return;
        }

        foreach (var folderItem in results)
        {
            var path = folderItem.Path.LocalPath;
            Directories.Add(path);
        }
    }

    private DelegateCommand? _delSelectedCmd;
    public DelegateCommand DelSelectedCmd => _delSelectedCmd ??= new(ExecuteDelSelectedCmd);

    private void ExecuteDelSelectedCmd()
    {
        if (SelectedDirectories is null)
        {
            return;
        }

        foreach (var item in SelectedDirectories.ToList())
        {
            SelectedDirectories.Remove(item);
        }
    }

    private DelegateCommand? _goNextCmd;
    public DelegateCommand GoNextCmd => _goNextCmd ??= new(ExecuteGoNextCmd);

    void ExecuteGoNextCmd()
    {
        _config.PdfExtensions.Clear();
        var pdfEx = _nonEditExtensions.Split(";");
        _config.PdfExtensions.AddRange(pdfEx);
        _config.EditableExtensions.Clear();
        var editEx = _editExtensions.Split(";");
        _config.EditableExtensions.AddRange(editEx);
        _config.DirectoriesToSearchIn.AddRange(Directories);

        _ctx.IndexerConfig = _config;
        _ctx.IndexDocuments();
    }
    
    public FileSystemIndexerConfigViewModel(
        IRegionManager regionManager,
        IContext ctx,
        IStorageProvider storageProvider)
    {
        _regionManager = regionManager;
        _ctx = ctx;
        _storageProvider = storageProvider;
        _config.DocumentEntries = _config.DocumentEntries;

        _nonEditExtensions = string.Join(";", _config.PdfExtensions);
        _editExtensions = string.Join(";", _config.EditableExtensions);
        DocsToFindCount = _config.DocumentEntries.Count;
    }
}