﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Abstractions.Controls;

namespace Nameless.WPF.Navigation;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="INavigationService"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterNavigationService(this IServiceCollection self) {
        self.TryAddSingleton<INavigationViewPageProvider, NavigationViewPageProvider>();
        self.TryAddSingleton<INavigationService, NavigationService>();

        return self;
    }

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
    public static IServiceCollection RegisterNavigationWindow(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(INavigationWindow);
        var implementations = assemblies.GetImplementations(service)
                                        .Where(type => !type.IsGenericTypeDefinition);

        foreach (var implementation in implementations) {
            var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);

            self.TryAdd(new ServiceDescriptor(service, implementation, lifetime));
        }

        return self;
    }

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
    public static IServiceCollection RegisterNavigableViews(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(INavigableView<>);
        var implementations = assemblies.GetImplementations(service)
                                        .Where(type => !type.IsGenericTypeDefinition);

        foreach (var implementation in implementations) {
            var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);
            var interfaces = implementation.GetInterfaces()
                                           .Where(@interface => @interface.GenericTypeArguments.Length > 0 &&
                                                                service.IsAssignableFromGenericType(@interface));
            foreach (var @interface in interfaces) {
                // Register the service as interface
                self.TryAdd(new ServiceDescriptor(@interface, implementation, lifetime));

                // Register the service view model
                var viewModelType = @interface.GetGenericArguments().First();
                self.TryAdd(new ServiceDescriptor(viewModelType, viewModelType, lifetime));
            }

            // Register the service as concrete type
            self.TryAdd(new ServiceDescriptor(implementation, implementation, lifetime));
        }

        return self;
    }

    public static IServiceCollection RegisterNavigationViewItemProvider(this IServiceCollection self, Assembly[] assemblies) {
        self.TryAddSingleton<INavigationViewItemProvider>(new DiscoverableNavigationViewItemProvider(assemblies));

        return self;
    }
}
