using System;
using System.ComponentModel;
using DocumentCollector.Infrastructure;
using Prism.Mvvm;

namespace DocumentCollector.SimpleSink;

public class FileSystemSinkConfigViewModel : BindableBase
{
    private readonly IContext _ctx;
    private string _sinkFolder = @"C:\temp\sink";

    public string SinkFolder
    {
        get => _sinkFolder;
        set => SetProperty(ref _sinkFolder, value);
    }

    private bool _groupBySource;

    public bool GroupBySource
    {
        get => _groupBySource;
        set => SetProperty(ref _groupBySource, value);
    }

    private bool _groupByExtension;

    public bool GroupByExtension
    {
        get => _groupByExtension;
        set => SetProperty(ref _groupByExtension, value);
    }

    public FileSystemSinkConfigViewModel(IContext ctx)
    {
        _ctx = ctx;
        if (_ctx.SinkConfiguration is not FileSystemSinkConfig)
        {
            _ctx.SinkConfiguration = new FileSystemSinkConfig();
        }
        PropertyChanged += OnPropertyChanged;
    }
    
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_ctx.SinkConfiguration is not FileSystemSinkConfig)
        {
            _ctx.SinkConfiguration = new FileSystemSinkConfig();
        }

        var sk = _ctx.SinkConfiguration as FileSystemSinkConfig;
        ArgumentNullException.ThrowIfNull(sk);
        sk.CopyToPath = _sinkFolder;
        sk.GroupByExtension = GroupByExtension;
        sk.GroupBySource = GroupBySource;
    }
}