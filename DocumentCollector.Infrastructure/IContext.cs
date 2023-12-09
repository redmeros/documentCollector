using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;

namespace DocumentCollector.Infrastructure;

public interface IContext
{
    #region LIST_READER
        public IDocumentListReaderDescriptor? SelectedListReaderDescriptor { get; set; }
        public IDocumentListReaderConfig? DocumentListReaderConfig { get; set; }
        public ICollection<DocumentEntry> DocumentEntries { get; set; }
        public ICollection<DocumentEntry> SelectedDocumentEntries { get; set; }
        public void ReadDocumentLists();
    #endregion

    #region INDEXER
        public IFileIndexerDescriptor? SelectedIndexerDescriptor { get; set; }
        public IFileIndexerConfig? IndexerConfig { get; set; }
        public void IndexDocuments();
        public FileIndex? FileIndex { get; set; }
    #endregion

    #region MATCHER
        public void MatchDocuments(IDocumentMatcher matcher);
        public ICollection<MatchResult>? MatchResults { get; set; }
    #endregion
}