using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Infrastructure.Services;

public interface ICommonDialogsService
{
    Task ShowError(Exception ex);
    (IProgress<T>, CancellationToken) ShowProgress<T>(string title);
    (IProgress<ProgressMessage>, CancellationToken) ShowInfiniteProgress(string message);
}