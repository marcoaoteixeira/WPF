using System.Net.Http.Headers;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nameless.Infrastructure;
using Nameless.IO.FileSystem;
using Nameless.Mediator;
using Nameless.Validation.FluentValidation;
using Nameless.WPF.Bootstrap;
using Nameless.WPF.Configuration;
using Nameless.WPF.Data;
using Nameless.WPF.GitHub;
using Nameless.WPF.Notifications;
using Nameless.WPF.UI;
using Nameless.WPF.UI.Dialogs.DirectorySelectionBox;
using Nameless.WPF.UI.Dialogs.MessageBox;
using Nameless.WPF.UI.Mvvm;
using Nameless.WPF.UI.Navigation;
using Nameless.WPF.UI.Snackbar;
using Nameless.WPF.UI.TaskRunner;
using Nameless.WPF.UI.Windows;
using Wpf.Ui;

namespace Nameless.WPF.Client;

/// <summary>
///     Entry point
/// </summary>
public partial class App {
    private static readonly Assembly[] SupportAssemblies = [
        typeof(ClientAssemblyMarker).Assembly,
        typeof(CoreAssemblyMarker).Assembly,
        typeof(UIAssemblyMarker).Assembly,
    ];

    private static readonly string[] Args = [
        $"--applicationName={Constants.Application.NAME}"
    ];

    private static readonly IHost CurrentHost = HostFactory.Create(Args)
                                                           .ConfigureServices(ConfigureServices)
                                                           .OnStartup(OnHostStartup)
                                                           .OnTearDown(OnHostTearDown)
                                                           .Build();

    // ReSharper disable once AsyncVoidMethod
    protected override async void OnStartup(StartupEventArgs e) {
        await CurrentHost.StartAsync();

        base.OnStartup(e);
    }

    // ReSharper disable once AsyncVoidMethod
    protected override async void OnExit(ExitEventArgs e) {
        await CurrentHost.StopAsync();

        CurrentHost.Dispose();

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
        // The basic
        services.AddOptions();
        services.ConfigureHttpClientDefaults(builder => {
            builder.ConfigureHttpClient((provider, client) => {
                var applicationContext = provider.GetRequiredService<IApplicationContext>();
                var productHeaderValue = new ProductHeaderValue(applicationContext.ApplicationName, applicationContext.Version);
                var userAgent = new ProductInfoHeaderValue(productHeaderValue);

                client.DefaultRequestHeaders.UserAgent.Add(userAgent);
            });
        });

        // From Third-party
        services.RegisterApplicationContext(opts => {
            opts.ApplicationName = Constants.Application.NAME;
            opts.Version = typeof(App).Assembly.GetName().Version ?? new Version(1, 0, 0);
        });
        services.RegisterMediator(opts => {
            opts.Assemblies = SupportAssemblies;
        });
        services.RegisterValidation(opts => {
            opts.Assemblies = SupportAssemblies;
        });

        // From Client
        services.RegisterContentDialogService();
        services.RegisterLogging();
        services.RegisterTimeProvider();

        // From Core
        services.RegisterAppConfigurationManager();
        services.RegisterAppDbContext(SupportAssemblies);
        services.RegisterBootstrapper(SupportAssemblies);
        services.RegisterFileSystem();
        services.RegisterNotificationService();
        services.RegisterGitHubHttpClient(configuration);

        // From UI
        services.RegisterDirectorySelectionBox();
        services.RegisterMessageBox();
        services.RegisterNavigableViews(SupportAssemblies);
        services.RegisterNavigationService();
        services.RegisterNavigationWindow(SupportAssemblies);
        services.RegisterSnackbarService();
        services.RegisterTaskRunner();
        services.RegisterViewModels(SupportAssemblies);
        services.RegisterWindowFactory(SupportAssemblies);
    }

    private static void OnHostStartup(IServiceProvider provider) {
        var bootstrapper = provider.GetRequiredService<IBootstrapper>()
                                   .ExecuteAsync(CancellationToken.None);

        Task.WaitAll(bootstrapper);

        provider.GetRequiredService<INavigationWindow>()
                .ShowWindow();
    }

    private static void OnHostTearDown(IServiceProvider provider) {
        var appConfigurationManager = provider.GetRequiredService<IAppConfigurationManager>()
                                              .SaveChangesAsync(CancellationToken.None);

        Task.WaitAll(appConfigurationManager);
    }
}
