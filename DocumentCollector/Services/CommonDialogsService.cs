using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Utils;
using DocumentCollector.Views;
using Prism.Services.Dialogs;

namespace DocumentCollector.Services;

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
                tcs.SetResult();
            });
        });
        return tcs.Task;
    }

    public (IProgress<T>, CancellationToken) ShowProgress<T>(string title)
    {
        var progress = new Progress<ProgressMessage>();
        var p = new DialogProgressParameters<ProgressMessage>()
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

    public (IProgress<ProgressMessage>, CancellationToken) ShowInfiniteProgress(string message)
    {
        var progress = new Progress<ProgressMessage>();
        var p = new DialogProgressParameters<ProgressMessage>()
        {
            Progress = progress
        };
        Dispatcher.UIThread.Post(() =>
        {
            _dialogService.ShowDialog(_mainWindow, DialogNames.InfiniteProgressDialog, p);
        });
        return (progress, p.CancellationTokenSource.Token);
    }
}