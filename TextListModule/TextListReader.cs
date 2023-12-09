using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;

namespace TextListModule;

public class TextListReader : IDocumentListReader
{
    public Task<IEnumerable<DocumentEntry>> Read(IProgress<ProgressMessage> progress, CancellationToken token = default!)
    {
        throw new NotImplementedException();
    }

    public void Configure(IDocumentListReaderConfig config)
    {
        throw new NotImplementedException();
    }
}