using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure;



public interface IDocumentListReader
{
    public Task<IEnumerable<DocumentEntry>> Read(IProgress<ReadProgressMessage> progress, CancellationToken token = default!);
    public void Configure(IDocumentListReaderConfig config);
}