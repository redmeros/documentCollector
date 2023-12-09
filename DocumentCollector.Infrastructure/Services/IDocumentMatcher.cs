using System.Text.RegularExpressions;
using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure.Services;

public interface IDocumentMatcher
{
    public string Key { get; }
    public string Display { get; }
    public string Description { get; }
    MatchResult MatchDocument(DocumentEntry entry, IFileIndex index);
}