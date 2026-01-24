using System.Net.Http.Headers;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nameless.Bootstrap;
using Nameless.Compression;
using Nameless.Diagnostics;
using Nameless.Infrastructure;
using Nameless.IO.FileSystem;
using Nameless.Mediator;
using Nameless.Validation.FluentValidation;
using Nameless.WPF.Behaviors;
using Nameless.WPF.Bootstrap;
using Nameless.WPF.Client.Bootstrap;
using Nameless.WPF.Client.Sqlite.Bootstrap.Steps;
using Nameless.WPF.Client.Sqlite.Data;
using Nameless.WPF.Client.Views.Windows;
using Nameless.WPF.Configuration;
using Nameless.WPF.Dialogs.FileSystem;
using Nameless.WPF.Dialogs.Message;
using Nameless.WPF.GitHub;
using Nameless.WPF.Mvvm;
using Nameless.WPF.Navigation;
using Nameless.WPF.Notifications;
using Nameless.WPF.Snackbar;
using Nameless.WPF.TaskRunner;
using Nameless.WPF.Windows;
using Wpf.Ui;

namespace Nameless.WPF.Client;

/// <summary>
///     Entry point
/// </summary>
public partial class App {
    private static readonly Assembly[] SupportAssemblies = [
        typeof(ClientAssemblyMarker).Assembly,
        typeof(CoreAssemblyMarker).Assembly,
        typeof(SqliteAssemblyMarker).Assembly
    ];

    private static readonly string[] Args = [
        $"--applicationName={Constants.Application.Name}"
    ];

    private static readonly IHost CurrentHost = HostFactory.Create(Args)
                                                           .ConfigureServices(ConfigureServices)
                                                           .OnStartup(OnHostStartup)
                                                           .OnTearDown(OnHostTearDown)
                                                           .Build();

    public App() { ExceptionWarden.Initialize(Constants.Application.Name); }

    // ReSharper disable once AsyncVoidEventHandlerMethod
    protected override async void OnStartup(StartupEventArgs e) {
        await CurrentHost.StartAsync()
                         .SkipContextSync();

        base.OnStartup(e);
    }

    // ReSharper disable once AsyncVoidEventHandlerMethod
    protected override async void OnExit(ExitEventArgs e) {
        await CurrentHost.StopAsync()
                         .SkipContextSync();

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
            opts.ApplicationName = Constants.Application.Name;
            opts.Version = typeof(App).Assembly.GetName().Version ?? new Version(1, 0, 0);
        });
        services.RegisterMediator(opts => {
            opts.Assemblies = SupportAssemblies;
            opts.RegisterRequestPipelineBehavior(typeof(PerformanceRequestPipelineBehavior<,>));
        });
        services.RegisterValidation(opts => {
            opts.Assemblies = SupportAssemblies;
        });
        services.RegisterActivitySourceProvider();

        // From Client
        services.RegisterContentDialogService();
        services.RegisterLogging();
        services.RegisterTimeProvider();

        // From Core
        services.RegisterAppConfigurationManager();
        services.RegisterAppDbContext(opts => {
            opts.RegisterInterceptor<AuditableEntitySaveChangesInterceptor>();
        });
        services.RegisterBootstrap(opts => {
            opts.RegisterStep<FirstFakeStep>();
            opts.RegisterStep<SecondFakeStep>();
            opts.RegisterStep<InitializeDbContextStep>();
        });
        services.RegisterFileSystem();
        services.RegisterNotificationService();
        services.RegisterGitHubHttpClient(configuration);
        services.RegisterFileSystemDialog();
        services.RegisterMessageDialog();
        services.RegisterNavigation(opts => {
            opts.Assemblies = SupportAssemblies;
        });
        services.RegisterSnackbarService();
        services.RegisterTaskRunner();
        services.RegisterViewModels(opts => {
            opts.Assemblies = SupportAssemblies;
        });
        services.RegisterWindowFactory(opts => {
            opts.Assemblies = SupportAssemblies;
        });
        services.RegisterZipArchiveService();
    }

    private static void OnHostStartup(IServiceProvider provider) {
        var main = provider.GetRequiredService<INavigationWindow>();

        provider.GetRequiredService<ISplashScreenWindow>()
                .Show(WindowStartupLocation.CenterScreen);

        main.ShowWindow();
    }

    // ReSharper disable once AsyncVoidMethod
    private static async void OnHostTearDown(IServiceProvider provider) {
        await provider.GetRequiredService<IAppConfigurationManager>()
                      .SaveChangesAsync(CancellationToken.None)
                      .SkipContextSync();
    }
}
