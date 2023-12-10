using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Services;
using DocumentCollector.Utils;

namespace DocumentCollector.SimpleSink;

public class FileSystemSinkDescriptor : ISinkDescriptor
{
    public string Key { get; } = SinkKeys.FileSystemSink;
    public string Display { get; } = "Ogólny zrzucacz";

    public string Description { get; } =
        "Zrzuca do wskazanego katalogu, oddzielnie pliki pdf i oddzielnie pliki edytowalne posegregowane po rozszerzeniach";
}

public class FileSystemSinkConfig : ISinkConfiguration
{
    public string CopyToPath { get; set; } = string.Empty;
    public bool GroupBySource { get; set; }
    public bool GroupByExtension { get; set; }
}

public class FileSystemSink : ISink
{
    private FileSystemSinkConfig? _config;

    public void Configure(ISinkConfiguration config)
    {
        if (config is not FileSystemSinkConfig cfg)
        {
            throw new CollectorException($"Wrong config type expected: {nameof(FileSystemSinkConfig)}");
        }
        _config = cfg;
    }
    
    public string SinkElements(IEnumerable<MatchResult> elementsToSink, IProgress<ProgressMessage> progress,
        CancellationToken ct)
    {
        var report = new StringBuilder();
        report.AppendLine("Rozpoczęto zrzucanie dokumentów");
        try
        {
            var elements = elementsToSink.ToList();
            var currentFile = 0;
            var totalFiles = elements.Count;
            foreach (var match in elements)
            {
                if (ct.IsCancellationRequested)
                {
                    return report.ToString();
                }

                ProcessElement(match, report);
                currentFile++;
                progress.Report(new ProgressMessage()
                {
                    JobDone = false,
                    Message = "Zrzucam dokumenty",
                    PercentageDone = currentFile / (float)totalFiles
                });
            }
        }
        finally
        {
            report.AppendLine("Zakończono zrzucanie dokumentów");
            progress.Report(new ProgressMessage()
            {
                JobDone = true
            });
        }
        return report.ToString();
    }

    private void ProcessElement(MatchResult match, StringBuilder report)
    {
        if (match.EditablePath is null)
        {
            report.AppendLine($"ERROR: Dla dokumentu {match.Entry.DocNo} brak pliku edytowalnego, wydanie z dnia: {match.Entry.IssueDate} nazwa: {match.Entry.Title}");
        }
        else
        {
            ProcessFile(match.EditablePath, match.Entry.Source, report);
        }

        if (match.NonEditablePath is null)
        {
            report.AppendLine($"ERROR: Dla dokumentu {match.Entry.DocNo} brak pliku nieedytowalnego, wydanie z dnia: {match.Entry.IssueDate} nazwa: {match.Entry.Title}");
        }
        else
        {
            ProcessFile(match.NonEditablePath, match.Entry.Source, report);
        }
    }

    private void ProcessFile(string sourcePath, string? source, StringBuilder report)
    {
        if (!File.Exists(sourcePath))
        {
            report.AppendLine($"ERROR: Nie mogę odnaleźć pliku {sourcePath}");
            return;
        }

        var destPath = GetDestFileName(Path.GetFileName(sourcePath), source);
        var destDir = Path.GetDirectoryName(destPath);
        ArgumentNullException.ThrowIfNull(destDir);
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        File.Copy(sourcePath, destPath, true);
    }

    private string GetDestFileName(string fileName, string? source)
    {
        ArgumentNullException.ThrowIfNull(_config);
        var basePath = _config.CopyToPath;
        if (_config.GroupBySource && source is not null)
        {
            basePath = Path.Join(basePath, source);
        }
        if (_config.GroupByExtension)
        {
            var ext = Path.GetExtension(fileName);
            basePath = Path.Join(basePath, ext.TrimStart('.'));
        }

        return Path.Join(basePath, fileName);
    }
}