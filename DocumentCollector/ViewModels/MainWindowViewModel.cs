using System;
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
        
        _manager.RequestNavigate(RegionNames.MainRegion, Step0ViewModel.NavKey, result =>
        {
            Console.WriteLine("First navigation request result: " + result.Result);
        });
    }
    
}