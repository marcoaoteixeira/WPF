using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Abstractions.Controls;

namespace Nameless.WPF.Navigation;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterNavigation(Action<NavigationOptions>? configure = null) {
            var innerConfigure = configure ?? (_ => { });
            var options = new NavigationOptions();

            innerConfigure(options);

            self.RegisterNavigationService();
            self.RegisterNavigationWindow(options.Assemblies);
            self.RegisterNavigableViews(options.Assemblies);
            self.RegisterNavigationViewItemProvider(options.Assemblies);

            return self;
        }

        private void RegisterNavigationService() {
            // Navigation services for WPF-UI
            self.TryAddSingleton<INavigationViewPageProvider, NavigationViewPageProvider>();
            self.TryAddSingleton<INavigationService, NavigationService>();
        }

        private void RegisterNavigationWindow(Assembly[] assemblies) {
            var service = typeof(INavigationWindow);

            // There should be just one navigation window.
            var navigationWindow = assemblies.GetImplementations(service)
                                             .SingleOrDefault(type => !type.IsGenericTypeDefinition);

            if (navigationWindow is null) {
                throw new InvalidOperationException($"There is not implementation of '{nameof(INavigationWindow)}' available.");
            }

            self.TryAdd(navigationWindow.CreateServiceDescriptor(service));
        }

        private void RegisterNavigableViews(Assembly[] assemblies) {
            var service = typeof(INavigableView<>);
            var implementations = assemblies.GetImplementations(service)
                                            .Where(type => !type.IsGenericTypeDefinition);

            foreach (var implementation in implementations) {
                var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);
                var interfaces = implementation.GetInterfaces()
                                               .Where(@interface => @interface.GenericTypeArguments.Length > 0 &&
                                                                    service.IsAssignableFromGenericType(@interface));
                foreach (var @interface in interfaces) {
                    // Register the navigable view's interface.
                    self.TryAdd(new ServiceDescriptor(@interface, implementation, lifetime));

                    // Register the navigable view viewmodel.
                    var viewModelType = @interface.GetGenericArguments().First();
                    self.TryAdd(new ServiceDescriptor(viewModelType, viewModelType, lifetime));
                }

                // Register the navigable view concrete type.
                self.TryAdd(new ServiceDescriptor(implementation, implementation, lifetime));
            }
        }

        private void RegisterNavigationViewItemProvider(Assembly[] assemblies) {
            self.TryAddSingleton<INavigationViewItemProvider>(new DiscoverableNavigationViewItemProvider(assemblies));
        }
    }
}