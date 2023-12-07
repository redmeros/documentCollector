using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DocumentCollector.ViewModels;
using DocumentCollector.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace DocumentCollector;

public partial class App : PrismApplication
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

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    // protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    // {
    //     base.ConfigureModuleCatalog(moduleCatalog);
    // }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void OnInitialized()
    {
        //register views with region manager
        base.OnInitialized();
    }
}