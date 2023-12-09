namespace DocumentCollector.Infrastructure.Models;

public class MatchResult
{
    public DocumentEntry Entry { get; init; }
    public string? EditablePath { get; set; }
    public string? NonEditablePath { get; set; }

    public MatchResult(DocumentEntry entry)
    {
        Entry = entry;
    }
}