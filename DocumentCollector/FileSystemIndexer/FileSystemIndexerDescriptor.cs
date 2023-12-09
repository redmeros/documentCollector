using System;
using DocumentCollector.Infrastructure;

namespace DocumentCollector.FileSystemIndexer;

public class FileSystemIndexerDescriptor : IFileIndexerDescriptor
{
    public string Key { get; } = "5F916DFE-3FB4-44ED-8D2B-133F93715E86";
    public string ConfigControlNavigationKey { get; } = "CC73438B-2E58-4624-9FE3-D92AE76A370D";
    public string Name { get; } = "Rekursywne wyszukiwanie w katalogach";
    public string Description { get; } = "Wyszukuje rekursywnie we wskazanych katalogach pliki edytowalne i pdf";
    public Type IndexerType { get; } = typeof(FileSystemIndexer);
}