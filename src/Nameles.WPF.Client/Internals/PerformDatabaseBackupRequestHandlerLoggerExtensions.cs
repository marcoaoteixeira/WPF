using Microsoft.Extensions.Logging;
using Nameless.WPF.Client.UseCases.Database.Backup;

namespace Nameless.WPF.Client.Internals;

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

    internal static void ExecuteDatabaseBackupFailure(this ILogger<PerformDatabaseBackupRequestHandler> self, Exception exception) {
        ExecuteDatabaseBackupFailureDelegate(self, exception);
    }

    internal static void PrepareBackupFileFailure(this ILogger<PerformDatabaseBackupRequestHandler> self, Exception exception) {
        PrepareBackupFileFailureDelegate(self, exception);
    }
}
