using Microsoft.Extensions.Logging;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.Internals;

internal static class WindowFactoryLoggerExtensions {
    private static readonly Action<ILogger, string, Exception?> WindowUnavailableDelegate
        = LoggerMessage.Define<string>(
            logLevel: LogLevel.Warning,
            eventId: default,
            formatString: "The window '{Window}' is not available. The error may be caused by the window not being registered in the dependency container."
        );

    internal static void WindowUnavailable(this ILogger<WindowFactory> self, Type windowType) {
        WindowUnavailableDelegate(self, windowType.Name, null /* exception */);
    }
}
