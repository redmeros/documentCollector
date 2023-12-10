using System;
using DocumentCollector.Infrastructure;

namespace ExcelListModule2;

public class ServiceDescriptor : IDocumentListReaderDescriptor
{
    public const string NameBase = "ExcelListReader";
    
    public string Key { get; } = "908AB37A-3D22-4A6D-9EEF-210A1328CDE5";
    public string Name { get; } = "Wiele standardowych list xlsx";
    
    public Type ReaderType { get; } = typeof(ExcelListDocumentReader);
}