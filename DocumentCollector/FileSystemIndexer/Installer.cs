using DocumentCollector.Infrastructure;
using Prism.Ioc;

namespace DocumentCollector.FileSystemIndexer;

public static class FileSystemIndexerInstaller
{
    public static IContainerRegistry AddFileSystemIndexer(this IContainerRegistry registry)
    {
        var descriptor = new FileSystemIndexerDescriptor();
        registry.RegisterInstance<IFileIndexerDescriptor>(descriptor, descriptor.Key);
        registry.Register<IFileIndexer, FileSystemIndexer>(descriptor.Key);
        registry.RegisterForNavigation<FileSystemIndexerConfigView, FileSystemIndexerConfigViewModel>(descriptor.Key);
        return registry;
    }
}