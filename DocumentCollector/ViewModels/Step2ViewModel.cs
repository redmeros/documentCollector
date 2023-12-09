using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Utils;
using DocumentCollector.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step2ViewModel : BindableBase
{
    private readonly IContext _ctx;
    private readonly IRegionManager _manager;
    private readonly ICommonDialogsService _dialogs;
    public const string NavKey = nameof(Step2View);

    public ObservableCollection<IFileIndexerDescriptor> Descriptors { get; } = new();
    
    public Step2ViewModel(
        IContext ctx,
        IRegionManager manager,
        ICommonDialogsService dialogs,
        IEnumerable<IFileIndexerDescriptor> descriptors)
    {
        _ctx = ctx;
        _manager = manager;
        _dialogs = dialogs;
        Descriptors.AddRange(descriptors);
    }

    private DelegateCommand<IFileIndexerDescriptor>? _selectedDescriptorCmd;
    public DelegateCommand<IFileIndexerDescriptor> SelectDescriptorCmd => _selectedDescriptorCmd ??= new DelegateCommand<IFileIndexerDescriptor>(ExecuteSelectedDescriptorCmd);

    private void ExecuteSelectedDescriptorCmd(IFileIndexerDescriptor parameter)
    {
        _ctx.SelectedIndexerDescriptor = parameter;
        _manager.RequestNavigate(RegionNames.MainRegion, parameter.ConfigControlNavigationKey, result =>
        {
            if (result.Result is false)
            {
                _dialogs.ShowError(new CollectorException("Błąd podczas nawigacji", result.Error));
            }
        });
    }
}