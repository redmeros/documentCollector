using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using DocumentCollector.FileSystemIndexer;
using DocumentCollector.Infrastructure;
using DocumentCollector.Infrastructure.Services;
using DocumentCollector.Kernel;
using DocumentCollector.Services;
using DocumentCollector.SimpleSink;
using DocumentCollector.Utils;
using DocumentCollector.ViewModels;
using DocumentCollector.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace DocumentCollector;

public class App : PrismApplication
{
    public static bool IsSingleViewLifetime => Environment.GetCommandLineArgs().Any(a => a == "--fbdev" || a == "-drm");

    public static AppBuilder BuildAvaloniaApp() => AppBuilder
        .Configure<App>()
        .UsePlatformDetect();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<MainWindowViewModel>();
        containerRegistry.RegisterSingleton<MainWindow>();

        containerRegistry.RegisterSingleton<IStorageProvider>(provider =>
        {
            var mw = provider.Resolve<MainWindow>();
            var topLevel = TopLevel.GetTopLevel(mw);
            if (topLevel is null)
            {
                throw new CollectorException("cannot get top level");
            }
            return topLevel.StorageProvider;
        });

        #region ServicesThatCanBeModules
            containerRegistry.AddFileSystemIndexer();
            containerRegistry.AddFileSystemSink();

            containerRegistry.Register<IDocumentMatcher, FilePathDocumentMatcher>();
            
        #endregion
        
        containerRegistry.RegisterSingleton<IContext, UiContext>();
        
        containerRegistry.RegisterDialog<ErrorDialog, ErrorDialogViewModel>();
        containerRegistry.RegisterDialog<ProgressDialog, ProgressDialogViewModel>(DialogNames.ProgressDialog);
        containerRegistry.RegisterDialog<InfiniteProgressDialog, ProgressDialogViewModel>(DialogNames.InfiniteProgressDialog);
        
        containerRegistry.RegisterSingleton<ICommonDialogsService, CommonDialogsService>();

        containerRegistry.RegisterForNavigation<Step0View, Step0ViewModel>(StepNames.Step0);
        containerRegistry.RegisterForNavigation<Step1View, Step1ViewModel>(StepNames.Step1);
        containerRegistry.RegisterForNavigation<Step2View, Step2ViewModel>(StepNames.Step2);
        containerRegistry.RegisterForNavigation<Step3View, Step3ViewModel>(StepNames.Step3);
        containerRegistry.RegisterForNavigation<Step4View, Step4ViewModel>(StepNames.Step4);
        containerRegistry.RegisterForNavigation<Step5View, Step5ViewModel>(StepNames.Step5);
    }

    protected override IModuleCatalog CreateModuleCatalog()
    {
        return new DirectoryModuleCatalog()
        { 
            ModulePath = @"./Modules" 
        };
    }
    
    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }
}