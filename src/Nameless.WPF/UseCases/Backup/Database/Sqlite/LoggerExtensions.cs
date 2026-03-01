using Microsoft.Extensions.Logging;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

internal static class LoggerExtensions {
    private static readonly Action<ILogger, Exception> ExecuteDatabaseBackupFailureDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while executing database backup."
        );

    extension(ILogger<PerformSqliteBackupRequestHandler> self) {
        internal void ExecuteDatabaseBackupFailure(Exception exception) {
            ExecuteDatabaseBackupFailureDelegate(self, exception);
        }
    }
}
