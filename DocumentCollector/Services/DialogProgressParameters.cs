using System;
using System.Threading;
using Prism.Services.Dialogs;

namespace DocumentCollector.Services;

public class DialogProgressParameters<T> : DialogParameters
{
    public Progress<T>? Progress { get; set; }

    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
}