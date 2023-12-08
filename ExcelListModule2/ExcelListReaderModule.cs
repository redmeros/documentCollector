using DocumentCollector.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;

namespace ExcelListModule2;

public class ExcelListReaderModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        var descriptor = new ServiceDescriptor();
        containerRegistry.RegisterInstance<IDocumentListReaderDescriptor>(descriptor, descriptor.Key);
        containerRegistry.Register<IDocumentListReader, ExcelListDocumentReader>(descriptor.Key);
        containerRegistry.RegisterForNavigation<ConfigureStepView, ConfigureStepViewModel>(descriptor.Step1NavigationKey);
    } 

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
}