using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.Resources;
using Wpf.Ui.Abstractions;

namespace Nameless.WPF.Navigation;

/// <summary>
///     Default implementation of <see cref="INavigationViewPageProvider"/>.
/// </summary>
public sealed class NavigationViewPageProvider : INavigationViewPageProvider {
    private readonly IServiceProvider _provider;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="NavigationViewPageProvider"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    public NavigationViewPageProvider(IServiceProvider provider) {
        _provider = Guard.Against.Null(provider);
    }

    /// <inheritdoc />
    public object GetPage(Type pageType) {
        if (typeof(FrameworkElement).IsAssignableFrom(pageType)) {
            return (FrameworkElement)_provider.GetRequiredService(pageType);
        }

        throw new InvalidOperationException(Exceptions.NavigationViewPageProvider_InvalidOperationException_PageMustBeFrameworkElement);
    }
}