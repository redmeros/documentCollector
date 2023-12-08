using DocumentCollector.Infrastructure;

namespace ExcelListModule2;

public class ExcelListDocumentReaderConfig : IDocumentListReaderConfig
{
    public string RegexCheck { get; set; } = @"^(\d{3})-(\S+)-(\S+)-(\S+)-(\S+)-(\d+)";
    public string StartColumn { get; set; } = "I";
    public string StartRow { get; set; } = "11";
    public static ExcelListDocumentReaderConfig Default => new ExcelListDocumentReaderConfig();

}