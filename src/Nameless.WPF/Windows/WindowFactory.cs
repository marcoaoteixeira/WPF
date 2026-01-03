using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Windows;

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
        _provider = provider;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool TryCreate<TWindow>([NotNullWhen(true)] out TWindow? output) where TWindow : IWindow {
        output = _provider.GetService<TWindow>();

        _logger.OnCondition(output is null).WindowUnavailable(typeof(TWindow));

        return output is not null;
    }
}