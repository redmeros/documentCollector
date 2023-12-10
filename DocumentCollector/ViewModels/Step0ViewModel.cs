using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentCollector.Infrastructure;
using DocumentCollector.Services;
using DocumentCollector.Views;
using Prism.Commands;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step0ViewModel
{
    private readonly IContext _ctx;
    private readonly IRegionManager _regionManager;
    public const string NavKey = nameof(Step0View);

    public ObservableCollection<IDocumentListReaderDescriptor> Descriptors { get; } = new();
    public Step0ViewModel(
        IContext ctx,
        IRegionManager regionManager,
        IEnumerable<IDocumentListReaderDescriptor> descriptors)
    {
        _ctx = ctx;
        _regionManager = regionManager;
        Descriptors.AddRange(descriptors);
    }

    private DelegateCommand<IDocumentListReaderDescriptor>? _selectedDescriptorCmd;
    public DelegateCommand<IDocumentListReaderDescriptor> SelectDescriptorCmd => _selectedDescriptorCmd ??= new(ExecuteSelectDescriptorCmd);

    private void ExecuteSelectDescriptorCmd(IDocumentListReaderDescriptor parameter)
    {
        _ctx.SelectedListReaderDescriptor = parameter;
        _regionManager.RequestNavigate(RegionNames.MainRegion, parameter.Key, result =>
        {
            Console.WriteLine(result);
        });
    }
}