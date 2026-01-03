using Microsoft.Extensions.Logging;
using Nameless.WPF.Configuration;

namespace Nameless.WPF.Internals;

/// <summary>
///     <see cref="AppConfigurationManager"/> logger extensions.
/// </summary>
internal static class AppConfigurationManagerLoggerExtensions {
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

    private static readonly Action<ILogger, Exception> LoadingConfigurationFailureDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while trying to load the configuration file."
        );

    extension(ILogger<AppConfigurationManager> self) {
        internal void SaveAppConfigurationFailure(Exception exception) {
            SaveAppConfigurationFailureDelegate(self, exception);
        }

        internal void TryGetValueFailure(string key, Type type, Exception exception) {
            TryGetValueFailureDelegate(self, key, type.Name, exception);
        }

        internal void LoadingConfigurationFailure(Exception exception) {
            LoadingConfigurationFailureDelegate(self, exception);
        }
    }
}
