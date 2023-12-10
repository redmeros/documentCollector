using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Utils;
using DocumentCollector.ViewModels;
using Prism.Ioc;
using Prism.Regions;

namespace DocumentCollector.Services;


public class UiContext : IContext
{
    private readonly IContainerProvider _container;
    private readonly IRegionManager _regionManager;
    private readonly ICommonDialogsService _dialogs;
    public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
    public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }
    public ICollection<DocumentEntry> DocumentEntries { get; set; } = new List<DocumentEntry>();
    public ICollection<DocumentEntry> SelectedDocumentEntries { get; set; } = new List<DocumentEntry>();
    public ICollection<MatchResult>? MatchResults { get; set; }
    public ISinkDescriptor? SelectedSinkDescriptor { get; set; }
    public ISinkConfiguration? SinkConfiguration { get; set; }
    public ICollection<MatchResult>? MatchesToSink { get; set; }
    
    public string? SinkReport { get; set; }
    
    public FileIndex? FileIndex { get; set; }
    public void MatchDocuments(IDocumentMatcher matcher)
    {
        var docCount = DocumentEntries.Count;
        var (progress, token) = _dialogs.ShowProgress<ProgressMessage>("Rozpoczynam dopasowywanie plików do dokumentów");
        Task.Run(async () =>
        {
            var results = new List<MatchResult>();
            var entries = DocumentEntries.ToList();
            if (FileIndex is null)
            {
                throw new CollectorException("Cannot process with null FileIndex");
            }
            for (var i = 0; i < DocumentEntries.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException("Operation cancelled by user");
                }
                progress.Report(new ProgressMessage()
                {
                    Message = $"Processing document {i+1} of {docCount}",
                    PercentageDone = (i + 1) / (float)docCount,
                    JobDone = false
                });
                
                var match = matcher.MatchDocument(entries[i], FileIndex);
                results.Add(match);
            }

            await Task.Delay(100);
            progress.Report(new ProgressMessage()
            {
                Message = "Job done",
                PercentageDone = -1,
                JobDone = true
            });
            Console.WriteLine("MatchDone");
            MatchResults = results;
            Dispatcher.UIThread.Post(() =>
            {
                _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step4);
            });
        });
    }

    public void SinkDocuments(Action<string>? callback)
    {
        if (SelectedSinkDescriptor is null)
        {
            throw new CollectorException("Selected sink descriptor is null");
        }

        var (progress, token) = _dialogs.ShowProgress<ProgressMessage>("Rozpoczynam zrzucanie dokumentów");
        Task.Run(() =>
        {
            var sink = _container.Resolve<ISink>(SelectedSinkDescriptor.Key);

            if (SinkConfiguration is not null)
            {
                sink.Configure(SinkConfiguration);
            }

            ArgumentNullException.ThrowIfNull(MatchesToSink);
            
            SinkReport = sink.SinkElements(MatchesToSink, progress, token);
            
            progress.Report(new ProgressMessage()
            {
                Message = "Job done",
                PercentageDone = -1,
                JobDone = true
            });
            if (callback is not null)
            {
                callback(SinkReport);
            }
        });

    }
    
    public void ReadDocumentLists()
    {
        if (SelectedListReaderDescriptor is null)
        {
            throw new Exception("SelectedListReaderDescriptor is null");
        }

        var (progress, token) =_dialogs.ShowProgress<ProgressMessage>("Rozpoczynam czytanie plików");
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
                    _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step1, result =>
                    {
                        Console.WriteLine("result");
                    });
                });
            }
            catch (Exception ex)
            {
                progress.Report(new ProgressMessage
                {
                    Message = "Błąd",
                    PercentageDone = -1,
                    JobDone = true,
                });
                await _dialogs.ShowError(ex);
            }
        });
    }

    public IFileIndexerDescriptor? SelectedIndexerDescriptor { get; set; }
    
    public IFileIndexerConfig? IndexerConfig { get; set; }
    public void IndexDocuments()
    {
        if (SelectedIndexerDescriptor is null)
        {
            throw new CollectorException("SelectedIndexerDescriptor is null");
        }

        var (progress, token) = _dialogs.ShowInfiniteProgress("Indeksuję dokumenty");
        Task.Run(async () =>
        {
            try
            {
                var indexer = _container.Resolve<IFileIndexer>(SelectedIndexerDescriptor.Key);

                if (IndexerConfig is not null)
                {
                    indexer.Configure(IndexerConfig);
                }

                FileIndex = await indexer.IndexDocuments(progress, token);

                // to jest ugly
                await Task.Delay(100);
                progress.Report(new ProgressMessage()
                {
                    Message = "Job done...",
                    PercentageDone = -1,
                    JobDone = true
                });
                
                Dispatcher.UIThread.Post(() =>
                {
                    _regionManager.RequestNavigate(RegionNames.MainRegion, StepNames.Step3);
                });
            }
            catch (Exception ex)
            {
                progress.Report(new ProgressMessage
                {
                    Message = "Błąd",
                    PercentageDone = -1,
                    JobDone = true,
                });
                await _dialogs.ShowError(ex);
            }
        });
    }

    public UiContext(
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