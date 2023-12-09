using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DocumentCollector.Infrastructure.Models;
using DocumentCollector.Services;
using DocumentCollector.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace DocumentCollector.ViewModels;

public class ProgressDialogViewModel : BindableBase, IDialogAware
{
    private CancellationTokenSource? _cancelSource;

    private Progress<ProgressMessage>? _progress;

    public Progress<ProgressMessage>? Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    private double _percentageDone;

    public double PercentageDone
    {
        get => _percentageDone;
        set => SetProperty(ref _percentageDone, value);
    }

    private string _message = "Rozpoczynam pracę";

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        if (_cancelSource is not null && !_cancelSource.IsCancellationRequested)
        {
            _cancelSource.Cancel();
        }
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        if (parameters is not DialogProgressParameters<ProgressMessage> p)
        {
            throw new CollectorException($"Wrong parameters type, expected: {typeof(DialogProgressParameters<ProgressMessage>)}");
        }

        _cancelSource = p.CancellationTokenSource;
        _cancelSource.Token.Register(() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Abort));
        });
        Progress = p.Progress;
        if (Progress is not null)
        {
            Progress.ProgressChanged += ProgressOnProgressChanged;
            Console.WriteLine("Subscribed to progress event");
        }
    }

    private void ProgressOnProgressChanged(object? sender, ProgressMessage e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            Console.WriteLine($"Got new message: {e}");
            Message = e.Message;
            PercentageDone = e.PercentageDone;
            if (e.JobDone)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }

        }, DispatcherPriority.MaxValue);
    }

    private DelegateCommand? _cancelCmd;
    public DelegateCommand CancelCmd => _cancelCmd ??= new(ExecuteCancelCmd);

    void ExecuteCancelCmd()
    {
        _cancelSource?.Cancel();
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    public string Title { get; } = "Postęp pracy";
    public event Action<IDialogResult>? RequestClose;
}