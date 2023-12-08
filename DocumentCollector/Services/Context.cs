using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using Prism.Ioc;

namespace DocumentCollector.Services;


public class Context : IContext
{
    private readonly IContainerProvider _container;
    private readonly ICommonDialogsService _dialogs;
    public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
    public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }

    public List<DocumentEntry> Entries = new();
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
                Entries = result.ToList();
                Console.WriteLine($"Got overall {Entries.Count} entries");
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
    public Context(IContainerProvider container, ICommonDialogsService dialogs)
    {
        Console.WriteLine("Creating context?");
        _container = container;
        _dialogs = dialogs;
    }
}