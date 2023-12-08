using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure;

public interface IDocumentListReaderConfig
{
}

public interface IDocumentListReader
{
    public IEnumerable<DocumentEntry> Read(Stream stream, IDocumentListReaderConfig? config);
    
}