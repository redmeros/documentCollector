using System;
using System.Collections.Generic;
using System.Linq;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step3ViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly IContext _ctx;
    private readonly ICommonDialogsService _dialogs;

    private int _indexedFilesCount;

    public int IndexedFilesCount
    {
        get => _indexedFilesCount;
        set => SetProperty(ref _indexedFilesCount, value);
    }

    private int _editableCount;

    public int EditableCount
    {
        get => _editableCount;
        set => SetProperty(ref _editableCount, value);
    }

    private int _nonEditableCount;

    public int NonEditableCount
    {
        get => _nonEditableCount;
        set => SetProperty(ref _nonEditableCount, value);
    }

    private int _documentsCount;

    public int DocumentsCount
    {
        get => _documentsCount;
        set => SetProperty(ref _documentsCount, value);
    }

    private readonly FileIndex _fileIndex;

    private DelegateCommand<IDocumentMatcher>? _matchDocumentsCmd;
    public DelegateCommand<IDocumentMatcher> MatchDocumentsCmd => _matchDocumentsCmd ??= new(ExecuteMatchDocumentsCmd);

    private void ExecuteMatchDocumentsCmd(IDocumentMatcher matcher)
    {
        try
        {
            _ctx.MatchDocuments(matcher);
        }
        catch (Exception ex)
        {
            _dialogs.ShowError(ex);
        }
    }
    
    private IDocumentMatcher? _selectedDocumentMatcher;

    public IDocumentMatcher? SelectedDocumentMatcher
    {
        get => _selectedDocumentMatcher;
        set => SetProperty(ref _selectedDocumentMatcher, value);
    }

    private List<IDocumentMatcher> _availableMatchers;

    public List<IDocumentMatcher> AvailableMatchers
    {
        get => _availableMatchers;
        set => SetProperty(ref _availableMatchers, value);
    }

    private DelegateCommand? _navigateBackCmd;
    public DelegateCommand NavigateBackCmd => _navigateBackCmd ??= new(ExecuteNavigateBackCmd);

    void ExecuteNavigateBackCmd()
    {
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step2);
    }
    
    public Step3ViewModel(
        IRegionManager regionManager,
        IContext ctx, 
        IEnumerable<IDocumentMatcher> matchers, 
        ICommonDialogsService dialogs)
    {
        Console.WriteLine("Step3ViewModel created");
        _regionManager = regionManager;
        _ctx = ctx;
        _dialogs = dialogs;
        _documentsCount = _ctx.DocumentEntries.Count;
        _fileIndex ??= _ctx.FileIndex ?? new FileIndex();
        _indexedFilesCount = _fileIndex.OverallCount;
        _editableCount = _fileIndex.EditableCount;
        _nonEditableCount = _fileIndex.NonEditableCount;
        _availableMatchers = matchers.OrderBy(w => w.Display).ToList();
        
    }
}