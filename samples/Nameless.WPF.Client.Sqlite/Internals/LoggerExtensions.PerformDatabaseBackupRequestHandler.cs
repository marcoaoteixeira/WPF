using Microsoft.Extensions.Logging;
using Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

namespace Nameless.WPF.Client.Sqlite.Internals;

internal static class PerformDatabaseBackupRequestHandlerLoggerExtensions {
    private static readonly Action<ILogger, Exception> ExecuteDatabaseBackupFailureDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while creating the database backup file."
        );

    private static readonly Action<ILogger, Exception> PrepareBackupFileFailureDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "An error occurred while preparing the database backup file for storage."
        );

    extension(ILogger<PerformDatabaseBackupRequestHandler> self)
    {
        internal void ExecuteDatabaseDataBackupFailure(Exception exception) {
            ExecuteDatabaseBackupFailureDelegate(self, exception);
        }

        internal void PrepareBackupFileFailure(Exception exception) {
            PrepareBackupFileFailureDelegate(self, exception);
        }
    }
}
