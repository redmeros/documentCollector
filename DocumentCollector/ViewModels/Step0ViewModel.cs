using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentCollector.Infrastructure;
using DocumentCollector.Services;
using DocumentCollector.Views;
using Prism.Commands;
using Prism.Regions;

namespace DocumentCollector.ViewModels;

public class Step0ViewModel : StepViewModelBase
{
    private readonly IContext _ctx;
    public const string NavKey = nameof(Step0View);

    public ObservableCollection<IDocumentListReaderDescriptor> Descriptors { get; } = new();
    public Step0ViewModel(
        IContext ctx,
        IRegionManager manager,
        IEnumerable<IDocumentListReaderDescriptor> descriptors) : base(manager, Step1ViewModel.NavKey, null)
    {
        _ctx = ctx;
        Descriptors.AddRange(descriptors);
    }

    private DelegateCommand<IDocumentListReaderDescriptor>? _selectedDescriptorCmd;
    public DelegateCommand<IDocumentListReaderDescriptor> SelectDescriptorCmd => _selectedDescriptorCmd ??= new(ExecuteSelectDescriptorCmd);

    private void ExecuteSelectDescriptorCmd(IDocumentListReaderDescriptor parameter)
    {
        _ctx.SelectedListReaderDescriptor = parameter;
        RegionManager.RequestNavigate(RegionNames.MainRegion, parameter.Step1NavigationKey, result =>
        {
            Console.WriteLine(result);
        });
    }
}