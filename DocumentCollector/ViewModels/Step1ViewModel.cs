using System.Collections.ObjectModel;
using System.Linq;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step1ViewModel : BindableBase
{
    private readonly IContext _ctx;
    private readonly IRegionManager _regionManager;
    public const string NavKey = StepNames.Step1;

    private int _entriesCount;

    public int EntriesCount
    {
        get => _entriesCount;
        set => SetProperty(ref _entriesCount, value);
    }
    public ObservableCollection<CheckableItem<DocumentEntry>> Entries { get; } = new();

    private DelegateCommand? _selectAllCmd;
    public DelegateCommand SelectAllCmd => _selectAllCmd ??= new(ExecuteSelectAllCmd);

    void ExecuteSelectAllCmd()
    {
        foreach (var checkableItem in Entries)
        {
            checkableItem.IsChecked = true;
        }
    }

    private DelegateCommand? _deselectAllCmd;
    public DelegateCommand DeselectAllCmd => _deselectAllCmd ??= new(ExecuteDeselectAllCmd);

    void ExecuteDeselectAllCmd()
    {
        foreach (var item in Entries)
        {
            item.IsChecked = false;
        }
    }

    private DelegateCommand? _invertSelectionCmd;
    public DelegateCommand InvertSelectionCmd => _invertSelectionCmd ??= new(ExecuteInvertSelectionCmd);

    void ExecuteInvertSelectionCmd()
    {
        foreach (var item in Entries)
        {
            item.IsChecked = !item.IsChecked;
        }
    }

    private DelegateCommand? _navigateNextCmd;
    public DelegateCommand NavigateNextCmd => _navigateNextCmd ??= new(ExecuteNavigateNextCmd);

    void ExecuteNavigateNextCmd()
    {
        foreach (var entry in Entries.Where(w => w.IsChecked).Select(w => w.Object).ToList())
        {
            _ctx.SelectedDocumentEntries.Add(entry);
        }
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step2);
    }

    private DelegateCommand? _navigateBackCmd;
    public DelegateCommand NavigateBackCmd => _navigateBackCmd ??= new(ExecuteNavigateBackCmd);

    void ExecuteNavigateBackCmd()
    {
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step0);
    }
    
    public Step1ViewModel(
        IContext ctx,
        IRegionManager regionManager
        )
    {
        _ctx = ctx;
        _regionManager = regionManager;
        EntriesCount = _ctx.DocumentEntries.Count;
        Entries.AddRange(_ctx.DocumentEntries.Select(w => new CheckableItem<DocumentEntry>(w)
        {
            IsChecked = true
        }));
    }
}
