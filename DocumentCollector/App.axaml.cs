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

        containerRegistry.Register<IDocumentMatcher, FilePathDocumentMatcher>(MatcherKeys.FilePathDocumentMatcher);
        
        containerRegistry.RegisterSingleton<IContext, UiContext>();
        
        containerRegistry.RegisterDialog<ErrorDialog, ErrorDialogViewModel>();
        containerRegistry.RegisterDialog<ProgressDialog, ProgressDialogViewModel>(DialogNames.ProgressDialog);
        containerRegistry.RegisterDialog<InfiniteProgressDialog, ProgressDialogViewModel>(DialogNames.InfiniteProgressDialog);
        
        containerRegistry.RegisterSingleton<ICommonDialogsService, CommonDialogsService>();

        containerRegistry.AddFileSystemIndexer();
        containerRegistry.RegisterForNavigation<Step0View, Step0ViewModel>();
        containerRegistry.RegisterForNavigation<Step1View, Step1ViewModel>();
        containerRegistry.RegisterForNavigation<Step2View, Step2ViewModel>();
        containerRegistry.RegisterForNavigation<Step3View, Step3ViewModel>(StepNames.Step3View);
        containerRegistry.RegisterForNavigation<Step4View, Step4ViewModel>(StepNames.Step4View);
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