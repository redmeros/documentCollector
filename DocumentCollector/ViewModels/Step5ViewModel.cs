using System;
using System.Collections.Generic;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ReactiveUI;

namespace DocumentCollector.ViewModels;

public class Step5ViewModel : BindableBase, INavigationAware
{
    private readonly IContext _ctx;
    private readonly IRegionManager _regionManager;
    private int _itemsToSinkCount;

    public int ItemsToSinkCount
    {
        get => _itemsToSinkCount;
        set => SetProperty(ref _itemsToSinkCount, value);
    }

    private IEnumerable<ISinkDescriptor> _availableSinkDescriptors;
    public IEnumerable<ISinkDescriptor> AvailableSinkDescriptors
    {
        get => _availableSinkDescriptors;
        set => SetProperty(ref _availableSinkDescriptors, value);
    }

    private ISinkDescriptor? _selectedSinkDescriptor;

    public ISinkDescriptor? SelectedSinkDescriptor
    {
        get => _selectedSinkDescriptor;
        set
        {
            if (SetProperty(ref _selectedSinkDescriptor, value) && _selectedSinkDescriptor is not null)
            {
                OnSinkDescriptorChanged(_selectedSinkDescriptor);
            }
        }
    }

    protected void OnSinkDescriptorChanged(ISinkDescriptor sinkDescriptor)
    {
        _regionManager.RequestNavigate(RegionNames.SinkSettingsRegion, sinkDescriptor.Key, result =>
        {
            Console.WriteLine(result.Result);
        });
        _ctx.SelectedSinkDescriptor = sinkDescriptor;
    }

    private DelegateCommand? _sinkCmd;
    public DelegateCommand SinkCmd => _sinkCmd ??= new(ExecuteSinkCmd);

    private string? _sinkReport;

    public string? SinkReport
    {
        get => _sinkReport;
        set => SetProperty(ref _sinkReport, value);
    }
    
    void ExecuteSinkCmd()
    {
        _ctx.SinkDocuments((s =>
        {
            SinkReport = s;
        }));
        
    }
    
    private DelegateCommand? _navigateBackCmd;
    public DelegateCommand NavigateBackCmd => _navigateBackCmd ??= new(ExecuteNavigateBackCmd);

    void ExecuteNavigateBackCmd()
    {
        _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step4);
    }
    
    public Step5ViewModel(
        IContext ctx,
        IRegionManager regionManager,
        IEnumerable<ISinkDescriptor> availableSinkDescriptors)
    {
        Console.WriteLine();
        _ctx = ctx;
        _regionManager = regionManager;
        _itemsToSinkCount = _ctx.MatchesToSink?.Count ?? 0;
        _availableSinkDescriptors = availableSinkDescriptors;
        
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        ItemsToSinkCount = _ctx.MatchesToSink?.Count ?? 0;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}