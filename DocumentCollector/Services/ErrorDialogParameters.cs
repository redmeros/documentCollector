using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services.Dialogs;

namespace DocumentCollector.Services;

public class ErrorDialogParameters : IDialogParameters
{
    public const string ErrKey = "Errors";
    
    public List<Exception> Errors { get; } = new();

    private bool IsErrKey(string key)
    {
        return string.Equals(key, ErrKey, StringComparison.OrdinalIgnoreCase);
    } 
    public void Add(string key, object value)
    {
        if (IsErrKey(key) && value is Exception ex)
        {
            Errors.Add(ex);
        }
    }

    public bool ContainsKey(string key)
    {
        return IsErrKey(key);
    }

    public T GetValue<T>(string key)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetValues<T>(string key)
    {
        if (typeof(T) == typeof(Exception))
        {
            return Errors.Cast<T>();
        }

        throw new NotImplementedException();
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        throw new NotImplementedException();
    }

    public int Count { get; } = 1;

    public IEnumerable<string> Keys { get; } = new[]
    {
        ErrKey
    };
}