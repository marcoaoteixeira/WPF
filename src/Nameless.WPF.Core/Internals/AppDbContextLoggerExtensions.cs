using Microsoft.Extensions.Logging;
using Nameless.WPF.Data;

namespace Nameless.WPF.Internals;

/// <summary>
///     Extension methods for <see cref="ILogger"/> that target
///     the <see cref="AppDbContext"/> implementations.
/// </summary>
internal static class AppDbContextLoggerExtensions {
    private static readonly Action<ILogger, Exception?> SkipMigrationForNonRelationalDatabaseDelegate
        = LoggerMessage.Define(
            logLevel: LogLevel.Information,
            eventId: default,
            formatString: "DbContext references a non-relational database, skipping migration action."
        );

    internal static void SkipMigrationForNonRelationalDatabase(this ILogger<AppDbContext> self) {
        SkipMigrationForNonRelationalDatabaseDelegate(self, null /* exception */);
    }
}
