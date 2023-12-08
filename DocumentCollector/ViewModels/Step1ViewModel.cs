using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DocumentCollector.Views;
using Prism.Commands;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step1ViewModel : StepViewModelBase
{
    public const string NavKey = nameof(Step1View);
    
    private readonly MainWindow _window;
    private ObservableCollection<string> _listPaths = new();

    public ObservableCollection<string> ListPaths
    {
        get => _listPaths;
        set => SetProperty(ref _listPaths, value);
    }

    [DesignOnly(true)]
    protected Step1ViewModel()
    {
        _window = null!;
        Console.WriteLine("This should not happen");
    }
    
    public Step1ViewModel(
        MainWindow window,
        IRegionManager regionManager) : base(regionManager, Step2ViewModel.NavKey, Step0ViewModel.NavKey)
    {
        _window = window;
        Console.WriteLine("step1viewmodel created");
    }
    
    private DelegateCommand? _addFolderToListPath;
    public DelegateCommand AddFolderToListPath => _addFolderToListPath ??= new(ExecuteAddFolderToListPath);
    async void ExecuteAddFolderToListPath()
    {
        var topLevel = TopLevel.GetTopLevel(_window);
        if (topLevel is null) return;
        var result = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Wybierz folder",
            });
        if (result is null || result.Count == 0)
        {
            return;
        }

        foreach (var folder in result)
        {
            await foreach (var item in folder.GetItemsAsync())
            {
                var path = Path.Join(item.Path.AbsolutePath, "");
                ListPaths.Add(path);
            }
        }
    }

    private DelegateCommand? _addFileToListPath;
    public DelegateCommand AddFileToListPath => _addFileToListPath ??= new DelegateCommand(ExecuteAddFileToListPath);

    private async void ExecuteAddFileToListPath()
    {
        var toplevel = TopLevel.GetTopLevel(_window);
        if (toplevel is null)
        {
            return;
        }

        var result = await toplevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions()
            {
                AllowMultiple = true,
                Title = "Wybierz plik",
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Excel files")
                    {
                        Patterns = new[] { "*.xlsx" }
                    }
                }
            });
        if (result is null || !result.Any())
        {
            return;
        }

        foreach (var file in result)
        {
            var path = Path.Join(file.Path.ToString());
            if (ListPaths.Contains(path))
            {
                continue;
            }

            ListPaths.Add(path);
        }
    }
    
    private DelegateCommand? _deletePathCmd;
    public DelegateCommand DeletePathCmd => _deletePathCmd ??= new DelegateCommand(ExecuteDeletePathCmd);

    private void ExecuteDeletePathCmd()
    {
        if (_selectedPaths is null)
        {
            return;
        }

        foreach (var path in _selectedPaths.OfType<string>().ToList())
        {
            ListPaths.Remove(path);
        }
    }
    
    private IList? _selectedPaths = new List<string>();

    public IList? SelectedPaths
    {
        get => _selectedPaths;
        set => SetProperty(ref _selectedPaths, value);
    }

    private DelegateCommand? _next;
    public DelegateCommand NextCmd => _next ??= new DelegateCommand(ExecuteNextCmd);

    private void ExecuteNextCmd()
    {
        NavigateNext();
    }
}

public class Step1ViewModelDesign : Step1ViewModel
{
    public Step1ViewModelDesign()
    {
        ListPaths.AddRange(Directory.GetFiles("C:\\temp\\714-SPISY"));
    }
}