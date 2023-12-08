using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Rendering;
using Avalonia.Threading;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.ViewModels;
using Prism.Ioc;
using Prism.Regions;

namespace DocumentCollector.Services;


public class Context : IContext
{
    private readonly IContainerProvider _container;
    private readonly IRegionManager _regionManager;
    private readonly ICommonDialogsService _dialogs;
    public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
    public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }
    public ICollection<DocumentEntry> DocumentEntries { get; set; }

    public ICollection<DocumentEntry> SelectedDocumentEntries { get; set; } = new List<DocumentEntry>();
    public void ReadDocumentLists()
    {
        if (SelectedListReaderDescriptor is null)
        {
            throw new Exception("SelectedListReaderDescriptor is null");
        }

        var (progress, token) =_dialogs.ShowProgress<ReadProgressMessage>("Rozpoczynam czytanie plików");
        Task.Run(async () =>
        {
            try
            {
                var reader = _container.Resolve<IDocumentListReader>(SelectedListReaderDescriptor.Key);
                if (DocumentListReaderConfig is not null)
                {
                    reader.Configure(DocumentListReaderConfig);
                }
                
                var result = await reader.Read(progress, token);
                DocumentEntries = result.ToList();
                Console.WriteLine($"Got overall {DocumentEntries.Count} entries");
                Dispatcher.UIThread.Post(() =>
                {
                    _regionManager.RequestNavigate(RegionNames.MainRegion, Step1ViewModel.NavKey, navigationResult =>
                    {
                        Console.WriteLine(navigationResult);
                    });
                });
            }
            catch (Exception ex)
            {
                progress.Report(new ReadProgressMessage
                {
                    Message = "Błąd",
                    PercentageDone = -1,
                    JobDone = true,
                });
                await _dialogs.ShowError(ex);
            }
        });
    }
    public Context(
        IContainerProvider container,
        IRegionManager regionManager,
        ICommonDialogsService dialogs)
    {
        Console.WriteLine("Creating context?");
        _container = container;
        _regionManager = regionManager;
        _dialogs = dialogs;
    }
}