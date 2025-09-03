using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Wpf.Ui;

namespace Nameless.WPF.Client;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterTimeProvider(this IServiceCollection self) {
        self.TryAddSingleton(TimeProvider.System);

        return self;
    }

    public static IServiceCollection RegisterContentDialogService(this IServiceCollection self) {
        self.TryAddSingleton<IContentDialogService, ContentDialogService>();

        return self;
    }

    public static IServiceCollection RegisterLogging(this IServiceCollection self) {
        self.AddLogging(configure => {
            configure.ClearProviders();
            configure.AddNLog();
        });

        return self;
    }
}
