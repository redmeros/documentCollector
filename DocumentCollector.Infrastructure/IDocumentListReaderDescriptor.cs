namespace DocumentCollector.Infrastructure;

public interface IDocumentListReaderDescriptor
{
    string Key { get; }
    string Name { get; }
    Type ReaderType { get; }
}