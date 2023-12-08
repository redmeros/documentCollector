using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure;

public interface IContext
{
    public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
    public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }
    public ICollection<DocumentEntry> DocumentEntries { get; set; }
    public ICollection<DocumentEntry> SelectedDocumentEntries { get; set; }
    public void ReadDocumentLists();
}