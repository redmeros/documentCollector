using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;

namespace ExcelListModule;

public class ExcelListReader : IDocumentListReader
{
    public IEnumerable<DocumentEntry> Read(Stream stream, IDocumentListReaderConfig? config)
    {
        throw new NotImplementedException();
    }
}