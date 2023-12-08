using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ExcelListModule2;

public class ConfigureStepViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly IContext _currentContext;
    private readonly ICommonDialogsService _dialogsService;
    private readonly IStorageProvider _storageProvider;
    private readonly ExcelListDocumentReaderConfig _currentConfig = new();

    private string _regex;

    public string Regex
    {
        get => _regex;
        set => SetProperty(ref _regex, value);
    }

    private string _startColumn;

    public string StartColumn
    {
        get => _startColumn;
        set => SetProperty(ref _startColumn, value);
    }

    private int _startRow;

    public int StartRow
    {
        get => _startRow;
        set => SetProperty(ref _startRow, value);
    }

    public ObservableCollection<string> Paths { get; } = new();

    public IList? SelectedPaths { get; set; } = new List<string>();
    
    public ConfigureStepViewModel(
        IRegionManager regionManager,
        IContext currentContext,
        ICommonDialogsService dialogsService,
        IStorageProvider storageProvider)
    {
        _regionManager = regionManager;
        _currentContext = currentContext;
        _dialogsService = dialogsService;
        _storageProvider = storageProvider;
        _currentConfig = ExcelListDocumentReaderConfig.Default;
        _regex = _currentConfig.RegexCheck;
        _startColumn = _currentConfig.StartColumn;
    }

    private DelegateCommand? _addFileCmd;
    public DelegateCommand AddFileCmd => _addFileCmd ??= new(ExecuteAddFileCmd);

    async void ExecuteAddFileCmd()
    {
        var opts = new FilePickerOpenOptions()
        {
            Title = "Wybierz pliki xlsx",
            AllowMultiple = true,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Pliki excel")
                {
                    Patterns = new[] { "*.xlsx" }
                }
            }
        };
        var results = await _storageProvider.OpenFilePickerAsync(opts);
        if (results is null || results.Count == 0)
        {
            return;
        }

        foreach (var item in results)
        {
            Paths.Add(Path.Join(item.Path.LocalPath));
        }
    }

    private DelegateCommand? _addFolderCmd;
    public DelegateCommand AddFolderCmd => _addFolderCmd ??= new(ExecuteAddFolderCmd);

    async void ExecuteAddFolderCmd()
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
            await foreach (var item in folderItem.GetItemsAsync())
            {
                var path = Path.GetFullPath(item.Path.LocalPath);
                if (string.Equals(Path.GetExtension(path), ".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    Paths.Add(path);
                }
            }
        }
    }

    private DelegateCommand? _delSelectedCmd;
    public DelegateCommand DelSelectedCmd => _delSelectedCmd ??= new(ExecuteDelSelectedCmd);

    void ExecuteDelSelectedCmd()
    {
        if (SelectedPaths is null)
        {
            return;
        }

        foreach (var item in SelectedPaths.OfType<string>().ToList())
        {
            Paths.Remove(item);
        }
    }

    private DelegateCommand? _goNextCmd;
    public DelegateCommand GoNextCmd => _goNextCmd ??= new(ExecuteGoNextCmd);

    private async void ExecuteGoNextCmd()
    {
        try
        {
            if (Paths.Count == 0)
            {
                throw new Exception("Nie wybrano żadnych plików list");
            }
            _currentConfig.RegexCheck = Regex;
            _currentConfig.StartColumn = StartColumn;
            _currentConfig.FilePaths = Paths;
            _currentContext.DocumentListReaderConfig = _currentConfig;
            
            _currentContext.ReadDocumentLists();

        }
        catch (Exception ex)
        {
            await _dialogsService.ShowError(ex);

        }
    }
}