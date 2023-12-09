using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure;

public interface IFileIndexer
{
    public Task<FileIndex> IndexDocuments(IProgress<ProgressMessage> progress, CancellationToken token = default);
    public void Configure(IFileIndexerConfig config);
}