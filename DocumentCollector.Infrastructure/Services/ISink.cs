using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure.Services;

public interface ISinkDescriptor
{
    public string Key { get; }
    public string Display { get; }
    public string Description { get; }
}

public interface ISinkConfiguration
{}

public interface ISink
{
    void Configure(ISinkConfiguration config);
    string SinkElements(IEnumerable<MatchResult> elementsToSink, IProgress<ProgressMessage> progress, CancellationToken ct);
}