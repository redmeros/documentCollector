﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DocumentCollector.Infrastructure;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager _manager;
    public MainWindowViewModel(IRegionManager manager)
    {
        Console.WriteLine("MainWindowViewModelConstructor");
        _manager = manager;
        TryNavigate();
    }

    [DesignOnly(true)]
    public MainWindowViewModel()
    {
        _manager = null!;
    }

    private async void TryNavigate()
    {
        _manager.Regions.ContainsRegionWithName(RegionNames.MainRegion);

        while (!_manager.Regions.ContainsRegionWithName(RegionNames.MainRegion))
        {
            await Task.Delay(100);
        }

        var region = _manager.Regions[RegionNames.MainRegion];
        region.NavigationService.NavigationFailed += NavigationServiceOnNavigationFailed;
        
        _manager.RequestNavigate(RegionNames.MainRegion, StepNames.Step0, result =>
        {
            Console.WriteLine("First navigation request result: " + result.Result);
        });
    }

    private void NavigationServiceOnNavigationFailed(object? sender, RegionNavigationFailedEventArgs e)
    {
        Console.WriteLine($"Navigation failed: {e.Error}");
    }
}