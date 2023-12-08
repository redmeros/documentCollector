using System.Collections;
using System.Collections.Generic;
using DocumentCollector.Infrastructure;

namespace ExcelListModule2;

public class ExcelListDocumentReaderConfig : IDocumentListReaderConfig
{
    public string RegexCheck { get; set; } = @"^(\d{3})-(\S+)-(\S+)-(\S+)-(\S+)-(\d+)";
    public string StartColumn { get; set; } = "I";

    public string TitleColumn { get; set; } = "P"; 
    public string SheetName { get; set; } = "SPIS DOKUMENTACJI";
    public static ExcelListDocumentReaderConfig Default => new ExcelListDocumentReaderConfig();
    public IEnumerable<string> FilePaths { get; set; } = new List<string>();

}