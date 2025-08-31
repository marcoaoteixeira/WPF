using Microsoft.Extensions.Logging;
using Nameless.WPF.Configuration;

namespace Nameless.WPF.Internals;

/// <summary>
///     <see cref="AppConfigurationManager"/> logger extensions.
/// </summary>
internal static class AppConfigurationManagerLoggerExtensions {
    private static readonly Action<ILogger, Exception?> AppConfigurationPhysicalFileNotFoundDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Warning,
            eventId: default,
            formatString: "Physical file for application configuration was not found."
        );

    private static readonly Action<ILogger, Exception> SaveAppConfigurationFailureDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while trying to save the application configuration file."
        );

    private static readonly Action<ILogger, string, string, Exception> TryGetValueFailureDelegate
        = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while trying to get the value for '{Key}' as '{Type}'."
        );

    internal static void AppConfigurationPhysicalFileNotFound(this ILogger<AppConfigurationManager> self) {
        AppConfigurationPhysicalFileNotFoundDelegate(self, null /* exception */);
    }

    internal static void SaveAppConfigurationFailure(this ILogger<AppConfigurationManager> self, Exception exception) {
        SaveAppConfigurationFailureDelegate(self, exception);
    }

    internal static void TryGetValueFailure(this ILogger<AppConfigurationManager> self, string key, Type type, Exception exception) {
        TryGetValueFailureDelegate(self, key, type.Name, exception);
    }
}
