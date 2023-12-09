using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;

namespace DocumentCollector.Kernel;

public class FilePathDocumentMatcher : IDocumentMatcher
{
    public string Key { get; } = MatcherKeys.FilePathDocumentMatcher;
    public string Display { get; } = "Standardowe dopasowanie";
    public string Description { get; } = "Dopasowuje numer dokumentu do nazwy pliku. Nazwa pliku musi zawierać w sobie dokładny numer dokumentu.";
    
    private Func<string, bool> Match(string docNo)
    {
        return path =>
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            return fileName.Contains(docNo, StringComparison.OrdinalIgnoreCase);
        };
    }
    
    public MatchResult MatchDocument(DocumentEntry entry, IFileIndex index)
    {
        var editableFiles = index.EditableIndex.Where(Match(entry.DocNo)).ToList();
        var nonEditableFiles = index.NonEditableIndex.Where(Match(entry.DocNo)).ToList();
        var matchResult = new MatchResult(entry);
        if (editableFiles.Any())
        {
            matchResult.EditablePath = editableFiles.First();
        }

        if (nonEditableFiles.Any())
        {
            matchResult.NonEditablePath = nonEditableFiles.First();
        }

        return matchResult;
    }
}