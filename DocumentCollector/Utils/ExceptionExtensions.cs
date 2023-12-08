using System;
using System.Collections.Generic;

namespace DocumentCollector.Utils;

public static class ExceptionExtensions
{
    public static List<string> Unwrap(this Exception ex)
    {
        var curException = ex;
        var messages = new List<string>();
        while (curException != null)
        {
            messages.Add(curException.Message);
            curException = curException.InnerException;
        }
        return messages;
    }
}