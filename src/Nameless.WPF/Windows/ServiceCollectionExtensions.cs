using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;

namespace Nameless.WPF.Windows;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    extension(IServiceCollection self) {
        /// <summary>
        ///     Registers the window services.
        /// </summary>
        /// <param name="configure">
        ///     Configure action for <see cref="WindowFactoryOptions"/>.
        /// </param>
        /// <returns>
        ///     The current <see cref="IServiceCollection"/> so other actions
        ///     can be chained.
        /// </returns>
        public IServiceCollection RegisterWindowFactory(Action<WindowFactoryOptions>? configure = null) {
            var innerConfigure = configure ?? (_ => { });
            var options = new WindowFactoryOptions();

            innerConfigure(options);

            var service = typeof(IWindow);
            var implementations = options.Assemblies
                                         .GetImplementations(service)
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
}