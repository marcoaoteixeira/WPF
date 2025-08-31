using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;

namespace Nameless.WPF.UI.Mvvm;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers all view models.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="assemblies">
    ///     The assemblies to scan for view model implementations.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterViewModels(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(IHasViewModel<>);
        var implementations = assemblies.GetImplementations(service)
                                        .Where(type => !type.IsGenericTypeDefinition);

        foreach (var implementation in implementations) {
            var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);
            var interfaces = implementation.GetInterfaces()
                                           .Where(@interface => @interface.GenericTypeArguments.Length > 0 &&
                                                                service.IsAssignableFromGenericType(@interface));

            foreach (var @interface in interfaces) {
                // Register the service view model
                var viewModelType = @interface.GetGenericArguments().First();

                // Registers the ViewModel type associated with the view
                self.TryAdd(new ServiceDescriptor(viewModelType, viewModelType, lifetime));
            }
        }

        return self;
    }
}
