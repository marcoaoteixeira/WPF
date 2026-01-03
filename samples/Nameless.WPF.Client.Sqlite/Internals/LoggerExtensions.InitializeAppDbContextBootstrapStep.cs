using Microsoft.Extensions.Logging;
using Nameless.WPF.Client.Sqlite.Bootstrap.Steps;
using Nameless.WPF.Client.Sqlite.Data;

namespace Nameless.WPF.Client.Sqlite.Internals;

/// <summary>
///     Extension methods for <see cref="ILogger"/> that target
///     the <see cref="AppDbContext"/> implementations.
/// </summary>
internal static class InitializeAppDbContextBootstrapStepLoggerExtensions {
    private static readonly Action<ILogger, Exception?> SkipMigrationForNonRelationalDatabaseDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Information,
            eventId: default,
            formatString: "DbContext references a non-relational database, skipping migration action."
        );

    internal static void SkipMigrationForNonRelationalDatabase(this ILogger<InitializeDbContextStep> self) {
        SkipMigrationForNonRelationalDatabaseDelegate(self, null /* exception */);
    }
}
