using System.ComponentModel;
using DocumentCollector.Infrastructure;
using DocumentCollector.Utils;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public abstract class StepViewModelBase : ViewModelBase
{
    protected IRegionManager RegionManager { get; }
    private readonly string? _nextViewNavKey;
    private readonly string? _previousViewNavKey;

    [DesignOnly(true)]
    protected StepViewModelBase()
    {
        RegionManager = null!;
    }
    
    protected StepViewModelBase(
        IRegionManager regionManager,
        string? nextNavigationKey,
        string? previousNavigationKey)
    {
        RegionManager = regionManager;
        _nextViewNavKey = nextNavigationKey;
        _previousViewNavKey = previousNavigationKey;
    }

    protected void NavigatePrevious()
    {
        if (_previousViewNavKey is null)
        {
            throw new CollectorException("Cannot navigate to null");
        }
        RegionManager.RequestNavigate(RegionNames.MainRegion, _previousViewNavKey);
    }
    
    protected void NavigateNext()
    {
        if (_nextViewNavKey is null)
        {
            throw new CollectorException("Cannot navigate to null");
        }
        RegionManager.RequestNavigate(RegionNames.MainRegion, _nextViewNavKey);
    }
}