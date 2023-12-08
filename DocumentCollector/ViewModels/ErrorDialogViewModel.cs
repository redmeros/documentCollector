using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DocumentCollector.Services;
using DocumentCollector.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace DocumentCollector.ViewModels;

public class ErrorDialogViewModel : BindableBase, IDialogAware
{
    public ObservableCollection<string> Messages { get; } = new();
    public bool CanCloseDialog()
    {
        return true;
    }
    public void OnDialogClosed()
    { }
    public void OnDialogOpened(IDialogParameters parameters)
    {
        if (parameters is not ErrorDialogParameters p)
        {
            Messages.Clear();
            return;
        }

        foreach (var ex in p.Errors)
        {
            Messages.AddRange(ex.Unwrap());
        }
        
    }

    public string Title { get; } = "Błąd";
    
    public event Action<IDialogResult>? RequestClose;

    private DelegateCommand? _closeCmd;
    public DelegateCommand CloseCmd => _closeCmd ??= new(ExecuteCloseCmd);

    void ExecuteCloseCmd()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
}