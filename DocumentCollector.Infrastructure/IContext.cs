namespace DocumentCollector.Infrastructure;

public interface IContext
{
    public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
    public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }
    public void ReadDocumentLists();
}