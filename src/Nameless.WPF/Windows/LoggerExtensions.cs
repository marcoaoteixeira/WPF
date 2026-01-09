using Microsoft.Extensions.Logging;

namespace Nameless.WPF.Windows;

internal static class LoggerExtensions {
    private static readonly Action<ILogger, string, Exception?> WindowUnavailableDelegate
        = LoggerMessage.Define<string>(
            logLevel: LogLevel.Warning,
            eventId: default,
            formatString: "The window '{Window}' is not available. The error may be caused by the window not being registered in the dependency container."
        );

    extension(ILogger<WindowFactory> self) {
        internal void WindowUnavailable(Type windowType) {
            WindowUnavailableDelegate(self, windowType.Name, null /* exception */);
        }
    }
}
