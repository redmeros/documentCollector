using System;
using DocumentCollector.Infrastructure;

namespace TextListModule;

public class ServiceDescriptor : IDocumentListReaderDescriptor
{
    public const string NameBase = "TextListReader";
    
    public string Step1NavigationKey { get; } = NameBase + "Step1";
    public string Step2NavigationKey { get; } = NameBase + "Step2";
    public string Step3NavigationKey { get; } = NameBase + "Step3";
    public string Key { get; } = "21A0346E-9179-4F02-B730-FC817DF66B61";
    public string Name { get; } = "Zwykła lista nazw dokumentów";
    public Type ReaderType { get; } = typeof(TextListReader);
}