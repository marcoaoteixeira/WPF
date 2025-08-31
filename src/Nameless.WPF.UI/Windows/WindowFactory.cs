using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.UI.Internals;

namespace Nameless.WPF.UI.Windows;

public class WindowFactory : IWindowFactory {
    private readonly IServiceProvider _provider;
    private readonly ILogger<WindowFactory> _logger;

    /// <summary>
    ///     Initializes a new instance of
    ///     the <see cref="WindowFactory"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public WindowFactory(IServiceProvider provider, ILogger<WindowFactory> logger) {
        _provider = Guard.Against.Null(provider);
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public bool TryCreate<TWindow>([NotNullWhen(true)] out TWindow? output) where TWindow : IWindow {
        output = _provider.GetService<TWindow>();

        _logger.OnCondition(output is null).WindowUnavailable(typeof(TWindow));

        return output is not null;
    }
}