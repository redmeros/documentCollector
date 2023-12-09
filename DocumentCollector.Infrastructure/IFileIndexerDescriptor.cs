namespace DocumentCollector.Infrastructure;

public interface IFileIndexerDescriptor
{
    string Key { get; }
    string Name { get; }
    
    string ConfigControlNavigationKey { get; }
    string Description { get; }
    Type IndexerType { get; }
}