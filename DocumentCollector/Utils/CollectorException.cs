using System;
using System.Runtime.Serialization;

namespace DocumentCollector.Utils;

public class CollectorException : Exception
{
    public CollectorException()
    {
    }

    protected CollectorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CollectorException(string? message) : base(message)
    {
    }

    public CollectorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}