using DocumentCollector.Infrastructure.Services;
using Prism.Ioc;

namespace DocumentCollector.SimpleSink;

public static class FileSystemSinkInstaller
{
    public static IContainerRegistry AddFileSystemSink(this IContainerRegistry registry)
    {
        var descriptor = new FileSystemSinkDescriptor();
        registry.RegisterInstance<ISinkDescriptor>(descriptor);
        registry.Register<ISink, FileSystemSink>(descriptor.Key);
        registry.RegisterForNavigation<FileSystemSinkConfigView, FileSystemSinkConfigViewModel>(descriptor.Key);
        return registry;
    }
}