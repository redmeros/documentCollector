using DocumentCollector.Views;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step2ViewModel : StepViewModelBase
{
    public const string NavKey = nameof(Step2View);
    public Step2ViewModel(IRegionManager manager) : base(manager, null, Step1ViewModel.NavKey)
    {
        
    }
}