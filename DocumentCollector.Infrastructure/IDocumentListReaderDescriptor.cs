namespace DocumentCollector.Infrastructure;

public interface IDocumentListReaderDescriptor
{
    string Key { get; }
    string Name { get; }
    
    string Step1NavigationKey { get; }
    Type ReaderType { get; }
}