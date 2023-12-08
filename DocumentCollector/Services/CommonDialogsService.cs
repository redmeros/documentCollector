using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Utils;
using DocumentCollector.Views;
using MathNet.Numerics.Distributions;
using Prism.Services.Dialogs;

namespace DocumentCollector.Services;

public class DialogProgressParameters<T> : DialogParameters
{
    public Progress<T>? Progress { get; set; }

    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
}

public class CommonDialogsService : ICommonDialogsService
{
    private readonly MainWindow _mainWindow;
    private readonly IDialogService _dialogService;

    public CommonDialogsService(MainWindow mainWindow, IDialogService dialogService)
    {
        _mainWindow = mainWindow;
        _dialogService = dialogService;
    }

    public Task ShowErrors(IEnumerable<Exception> exs)
    {
        throw new NotImplementedException();
    }
    
    public Task ShowError(Exception ex)
    {
        var p = new ErrorDialogParameters();
        var tcs = new TaskCompletionSource();
        p.Add(ErrorDialogParameters.ErrKey, ex);
        Dispatcher.UIThread.Post(() =>
        {
            _dialogService.ShowDialog(_mainWindow, DialogNames.ErrorDialog, p, _ =>
            {
                var visualDescendants = TopLevel.GetTopLevel(_mainWindow).GetVisualDescendants();
                var children = TopLevel.GetTopLevel(_mainWindow).GetVisualChildren();
                tcs.SetResult();
            });
        });
        return tcs.Task;
    }

    public (IProgress<T>, CancellationToken) ShowProgress<T>(string title)
    {
        var progress = new Progress<ReadProgressMessage>();
        var p = new DialogProgressParameters<ReadProgressMessage>()
        {
            Progress = progress
        };
        Dispatcher.UIThread.Post(() =>
        {
            _dialogService.ShowDialog(_mainWindow, DialogNames.ProgressDialog, p);
        }, DispatcherPriority.Render);
        
        if (progress is IProgress<T> iProgress)
        {
            return (iProgress, p.CancellationTokenSource.Token);
        }

        throw new CollectorException("cannot create progress");
    }
}