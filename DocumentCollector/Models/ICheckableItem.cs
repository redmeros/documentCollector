using Prism.Mvvm;

namespace DocumentCollector.Models;

public class CheckableItem<T> : BindableBase where T: class
{
    private T _object;

    public T Object
    {
        get => _object;
        set => SetProperty(ref _object, value);
    }

    private bool _isChecked;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
    public CheckableItem(T item)
    {
        _object = item;
    }
}