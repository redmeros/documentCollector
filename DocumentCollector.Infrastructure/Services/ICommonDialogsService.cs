using System;
using Prism.Services.Dialogs;

namespace DocumentCollector.Services;

public interface ICommonDialogsService
{
    void ShowError(Exception ex, Action<IDialogResult>? callback = null);
}