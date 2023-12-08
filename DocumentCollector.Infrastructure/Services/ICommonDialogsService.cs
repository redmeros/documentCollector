namespace DocumentCollector.Infrastructure.Services;

public interface ICommonDialogsService
{
    Task ShowError(Exception ex);
    (IProgress<T>, CancellationToken) ShowProgress<T>(string title);
}