using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Utils;

namespace DocumentCollector.FileSystemIndexer;

public class FileSystemIndexer : IFileIndexer
{
    private FileSystemIndexerConfig? _config;
    
    public Task<FileIndex> IndexDocuments(IProgress<ProgressMessage> progress, CancellationToken token = default)
    {
        var idx = new FileIndex();

        if (_config is null)
        {
            throw new CollectorException("Indexer config is null");
        }
        
        for (var i = 0; i < _config.DirectoriesToSearchIn.Count; i++)
        {
            var dirPath = _config.DirectoriesToSearchIn[i];
            try
            {
                progress.Report(new ProgressMessage()
                {
                    Message = $"Searching {i + 1}/{_config.DirectoriesToSearchIn.Count} - ({dirPath})",
                    JobDone = false,
                    PercentageDone = -1
                });
                var files = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file);
                    if (_config.PdfExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        idx.TryAddNonEditable(file);
                    }
                    else if (_config.EditableExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        idx.TryAddEditable(file);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during indexing directory: {dirPath}", ex);
            }
        }
        return Task.FromResult(idx);
    }

    public void Configure(IFileIndexerConfig config)
    {
        if (config is FileSystemIndexerConfig cfg)
        {
            _config = cfg;
        }
    }
}