using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;

namespace Nameless.WPF.UI.Windows;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the windows services.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="assemblies">
    ///     A collection of assemblies to scan
    ///     for <see cref="IWindow"/> implementations.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterWindowFactory(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(IWindow);
        var implementations = assemblies.GetImplementations(service)
                                        .Where(type => !type.IsGenericTypeDefinition);

        foreach (var implementation in implementations) {
            var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);
            var interfaces = implementation.GetInterfaces()
                                           .Where(service.IsAssignableFrom);

            foreach (var @interface in interfaces) {
                // Skip the main service interface
                if (@interface == service) {
                    continue;
                }

                // Register the service with its extended interface
                self.TryAdd(new ServiceDescriptor(@interface, implementation, lifetime));
            }

            // Register the service as concrete type
            self.TryAdd(new ServiceDescriptor(implementation, implementation, lifetime));
        }

        self.TryAddSingleton<IWindowFactory, WindowFactory>();

        return self;
    }
}
