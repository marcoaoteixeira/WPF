using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Bootstrap;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the bootstrap service and its steps.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="assemblies">
    ///     The assemblies to scan for bootstrap steps.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterBootstrapService(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(BootstrapStep);
        var implementations = assemblies.GetImplementations(service)
                                        .Where(type => !type.IsGenericTypeDefinition)
                                        .Select(type => new ServiceDescriptor(service, type, ServiceLifetime.Singleton))
                                        .ToArray();

        self.TryAddEnumerable(implementations);
        self.TryAddSingleton<IBootstrapper, Bootstrapper>();

        return self;
    }
}
