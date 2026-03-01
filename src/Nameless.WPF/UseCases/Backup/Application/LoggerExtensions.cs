using Microsoft.Extensions.Logging;

namespace Nameless.WPF.UseCases.Backup.Application;

internal static class LoggerExtensions {
    private static readonly Action<ILogger, string, Exception?> ExecuteApplicationBackupFailureDelegate
        = LoggerMessage.Define<string>(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while executing application backup. {Reason}"
        );

    extension(ILogger<PerformApplicationBackupRequestHandler> self) {
        internal void ExecuteApplicationBackupFailure(string reason) {
            ExecuteApplicationBackupFailureDelegate(self, reason, null /* exception */);
        }
    }
}
