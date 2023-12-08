﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace ExcelListModule2;

public class ExcelListDocumentReader : IDocumentListReader
{
    private ExcelListDocumentReaderConfig _config = ExcelListDocumentReaderConfig.Default;
    public async Task<IEnumerable<DocumentEntry>> Read(IProgress<ReadProgressMessage> progress, CancellationToken token = default!)
    {
        Console.WriteLine("waiting");
        await Task.Delay(10, token);
        var files = _config.FilePaths.ToList();
        var maxFiles = files.Count;
        
        var allDocs = new List<DocumentEntry>();

        for (var i = 0; i < maxFiles; i++) 
        {
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException("Cancelled by user");
            }
            
            var docs = ReadFile(files[i], token);
            
            var percent =  (i + 1) / maxFiles;
            var msg = new ReadProgressMessage
            {
                Message = $"Processing... {i + 1} of {maxFiles} which is ({percent * 100}%)",
                PercentageDone = percent, JobDone = false
            };
            progress.Report(msg);
            allDocs.AddRange(docs);
            Console.WriteLine($"File: {files[i]} read. Got {docs.Count} document entries"); 
        }
        progress.Report(new ReadProgressMessage
        {
            Message = "Job done",
            PercentageDone = 1f,
            JobDone = true
        });
        return allDocs;
    }

    public List<DocumentEntry> ReadFile(string path, CancellationToken ct)
    {
        using var stream = File.OpenRead(path);
        using var wb = new XSSFWorkbook(stream);
        
        // używam pierwszego
        var sheet = wb.GetSheet(_config.SheetName);
        var lastRow = sheet.LastRowNum;
        
        var cellRef = new CellReference(_config.StartColumn + "1");
        
        var docNoColIndex = cellRef.Col;
        var cellRefTitle = new CellReference(_config.TitleColumn + "1");
        var docTitleColIndex = cellRefTitle.Col;

        var source = Path.GetFileNameWithoutExtension(path);

        var result = new List<DocumentEntry>();
        
        for (var i = 0; i < lastRow; i++)
        {
            var row = sheet.GetRow(i);

            var cell = row?.GetCell(docNoColIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            var docNoValue = cell?.StringCellValue;
            if (docNoValue is null)
            {
                continue;
            }
            if (!IsDocumentRow(docNoValue))
            {
                continue;
            }
            
            if (IsHiddenRow(row))
            {
                continue;
            }
            var docEntry = new DocumentEntry()
            {
                DocNo = docNoValue,
                Source = source,
                Title = SafeGetStringValue(row, docTitleColIndex)
            };
            result.Add(docEntry);
        }
        return result;
    }

    private bool IsHiddenRow(IRow? row)
    {
        if (row is null)
        {
            return true;
        }

        if (row.IsFormatted && row.RowStyle.IsHidden)
        {
            return true;
        }

        return row.ZeroHeight;
    }
    
    public string SafeGetStringValue(IRow? row, short column)
    {
        if (row is null)
        {
            return "No title for row";
        }
        var cell = row.GetCell(column);
        if (cell is null)
        {
            return "No title for cell";
        }
        var value = cell.StringCellValue;
        return value ?? "No title for string";
    }

    private Regex? rx;
    public bool IsDocumentRow(string docNoValue)
    {
        rx ??= new Regex(_config.RegexCheck, RegexOptions.Compiled);
        return rx.IsMatch(docNoValue);
    }
    
    public void Configure(IDocumentListReaderConfig config)
    {
        if (config is ExcelListDocumentReaderConfig cfg)
        {
            _config = cfg;
            return;
        }

        throw new Exception($"Wrong config type provided, expected {typeof(ExcelListDocumentReaderConfig)}");

    }
}