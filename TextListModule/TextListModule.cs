﻿using System;
using DocumentCollector.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;

namespace TextListModule;

public class TextListModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        var descriptor = new ServiceDescriptor();
        containerRegistry.RegisterInstance<IDocumentListReaderDescriptor>(descriptor, descriptor.Key);
        containerRegistry.Register<IDocumentListReader, TextListReader>(descriptor.Key);
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        Console.WriteLine("test");
    }
}