using System.Collections.Generic;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.FileSystemIndexer;

public class FileSystemIndexerConfig : IFileIndexerConfig
{
    public ICollection<DocumentEntry> DocumentEntries { get; set; } = new List<DocumentEntry>();
    public List<string> DirectoriesToSearchIn { get; } = new();
    public List<string> PdfExtensions { get; } = new()
    {
        ".pdf"
    };
    public List<string> EditableExtensions { get; } = new()
    {
        ".dwg",
        ".docx",
        ".doc",
        ".xlsx",
        ".xls"
    };
}