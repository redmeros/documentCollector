using System;
using System.Collections.Generic;
using System.IO;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;

namespace TextListModule;

public class TextListReader : IDocumentListReader
{
    public IEnumerable<DocumentEntry> Read(Stream stream, IDocumentListReaderConfig? config)
    {
        throw new NotImplementedException();
    }
}