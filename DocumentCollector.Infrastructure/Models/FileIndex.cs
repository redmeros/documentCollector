namespace DocumentCollector.Infrastructure.Models;

public interface IFileIndex
{
    bool TryAddEditable(string path);
    bool TryAddNonEditable(string path);

    int OverallCount { get; }
    int EditableCount { get; }
    int NonEditableCount { get; }
    
    IEnumerable<string> EditableIndex { get; }
    IEnumerable<string> NonEditableIndex { get; }
}

public class FileIndex : IFileIndex
{
    private HashSet<string> EditablePaths { get; } = new();
    private HashSet<string> NonEditablePaths { get; } = new();

    public IEnumerable<string> EditableIndex => EditablePaths;
    public IEnumerable<string> NonEditableIndex => NonEditablePaths;

    public bool TryAddEditable(string path)
    {
        return EditablePaths.Add(path);
    }

    public bool TryAddNonEditable(string path)
    {
        return NonEditablePaths.Add(path);
    }

    public int OverallCount => EditableCount + NonEditableCount;
    public int EditableCount => EditablePaths.Count;
    public int NonEditableCount => NonEditablePaths.Count;
}