using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DocumentCollector.Infrastructure.Models;

namespace DocumentCollector.Models;

// tego tak naprawdę nie potrzebuje 
// ja chcę tylko mieć zaznaczone elementy
// dlatego trzeba pomyśleć nad czymś innym typu - ICheckableItem<T>
// kolumny i tak się robi zazwyczaj samemu
public class DocumentEntryViewModel : DocumentEntry, INotifyPropertyChanged
{
    private string _docNo = string.Empty;
    public override string DocNo
    {
        get => _docNo;
        set => SetProperty(ref _docNo, value);
    }

    private string _title = string.Empty;
    public override string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _source = string.Empty;
    public override string Source
    {
        get => _source;
        set => SetProperty(ref _source, value);
    }

    private DateTime _issueDate = DateTime.MinValue;
    public override DateTime IssueDate
    {
        get => _issueDate;
        set => SetProperty(ref _issueDate, value);
    }

    public DocumentEntryViewModel(){}
    public DocumentEntryViewModel(DocumentEntry entry)
    {
        _docNo = entry.DocNo;
        _title = entry.Title;
        _source = entry.Source;
        _issueDate = entry.IssueDate;
    }
    
    #region INotifyPropertyChangeImplementation

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        RaisePropertyChanged(propertyName);

        return true;
    }
    
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }
    

    #endregion
}