namespace DocumentCollector.Infrastructure.Models;

public class ReadProgressMessage
{
    public string Message { get; init; } = string.Empty;
    public double PercentageDone { get; init; } 
    
    public bool JobDone { get; init; }

    public override string ToString()
    {
        return $"[ReadProgressMessage] Percentage: {PercentageDone} JobDone: {JobDone} Message: {Message}";
    }
}