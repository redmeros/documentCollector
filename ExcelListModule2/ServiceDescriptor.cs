using DocumentCollector.Infrastructure;

namespace ExcelListModule;

public class ServiceDescriptor : IDocumentListReaderDescriptor
{
    public string Key { get; } = "908AB37A-3D22-4A6D-9EEF-210A1328CDE5";
    public string Name { get; } = "Wiele standardowych list xlsx";
    public Type ReaderType { get; }
}