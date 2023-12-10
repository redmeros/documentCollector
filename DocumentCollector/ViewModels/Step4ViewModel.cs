using System;
using System.Collections.Generic;
using System.Linq;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Models;
using DocumentCollector.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step4ViewModel : BindableBase
{
    private readonly IContext _ctx;
    private readonly IRegionManager _regionManager;

    public ICollection<CheckableItem<MatchResult>> MatchResults { get; init; }

    private DelegateCommand? _selectAllCmd;
    public DelegateCommand SelectAllCmd => _selectAllCmd ??= new(ExecuteSelectAllCmd);

    void ExecuteSelectAllCmd()
    {
        foreach (var item in MatchResults)
        {
            item.IsChecked = true;
        }
    }

    private DelegateCommand? _unselectAllCmd;
    public DelegateCommand UnselectAllCmd => _unselectAllCmd ??= new(ExecuteUnselectAllCmd);

    void ExecuteUnselectAllCmd()
    {
        foreach (var item in MatchResults)
        {
            item.IsChecked = false;
        }
    }

    private DelegateCommand? _invertSelectionCmd;
    public DelegateCommand InvertSelectionCmd => _invertSelectionCmd ??= new(ExecuteInvertSelectionCmd);

    void ExecuteInvertSelectionCmd()
    {
        foreach (var item in MatchResults)
        {
            item.IsChecked = !item.IsChecked;
        }
    }

    private DelegateCommand? _copyToCmd;
    public DelegateCommand CopyToCmd => _copyToCmd ??= new(ExecuteCopyToCmd);

    void ExecuteCopyToCmd()
    {
        _ctx.MatchesToSink = MatchResults.Where(w => w.IsChecked).Select(w => w.Object) .ToList();
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step5);
    }
    
    private DelegateCommand? _navigateBackCmd;
    public DelegateCommand NavigateBackCmd => _navigateBackCmd ??= new(ExecuteNavigateBackCmd);

    void ExecuteNavigateBackCmd()
    {
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step3);
    }
    
    
    public Step4ViewModel(IContext ctx, IRegionManager regionManager)
    {
        _ctx = ctx;
        _regionManager = regionManager;
        if (_ctx.MatchResults is null)
        {
            throw new ArgumentNullException();
        }
        MatchResults = _ctx.MatchResults.Select(w => new CheckableItem<MatchResult>(w) { IsChecked = !w.HasError}).ToList();
        Console.WriteLine("Step4Created");
    }
}