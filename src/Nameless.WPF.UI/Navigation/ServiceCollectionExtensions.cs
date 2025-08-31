using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace Nameless.WPF.UI.Navigation;

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
}
