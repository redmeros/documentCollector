using Microsoft.VisualBasic;

namespace DocumentCollector.Infrastructure.Models;

public class DocumentEntry
{
    public virtual string DocNo { get; set; } = string.Empty;
    public virtual string Title { get; set; } = string.Empty;
    public virtual string Source { get; set; } = string.Empty;
    public virtual DateTime IssueDate { get; set; } = DateTime.MinValue;
}