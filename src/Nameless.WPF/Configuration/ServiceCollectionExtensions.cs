using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Configuration;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="IAppConfigurationManager"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterAppConfigurationManager(this IServiceCollection self) {
        Guard.Against.Null(self);

        self.TryAddSingleton<IAppConfigurationManager, AppConfigurationManager>();

        return self;
    }
}
