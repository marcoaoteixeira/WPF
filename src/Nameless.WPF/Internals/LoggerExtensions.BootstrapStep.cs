using Microsoft.Extensions.Logging;
using Nameless.WPF.Bootstrap;

namespace Nameless.WPF.Internals;

/// <summary>
///     Extension methods for <see cref="ILogger"/> that target
///     the <see cref="Bootstrapper"/> implementations.
/// </summary>
internal static class BootstrapperLoggerExtensions {
    private static readonly Action<ILogger, string, Exception?> StartingExecutionDelegate
        = LoggerMessage.Define<string>(
            logLevel: LogLevel.Information,
            eventId: default,
            formatString: "Starting execution of bootstrap step '{Step}'..."
        );

    private static readonly Action<ILogger, string, Exception> ExecutionFailureDelegate
        = LoggerMessage.Define<string>(
            logLevel: LogLevel.Error,
            eventId: default,
            formatString: "Execution of bootstrap step '{Step}' failed."
        );

    private static readonly Action<ILogger, string, long, Exception?> ExecutionFinishedDelegate
        = LoggerMessage.Define<string, long>(
            logLevel: LogLevel.Information,
            eventId: default,
            formatString: "Execution of bootstrap step '{Step}' finished in {Duration}ms."
        );

    internal static void StartingExecution(this ILogger<Bootstrapper> self, BootstrapStep step) {
        StartingExecutionDelegate(self, step.Name, null /* exception */);
    }

    internal static void ExecutionFailure(this ILogger<Bootstrapper> self, BootstrapStep step, Exception exception) {
        ExecutionFailureDelegate(self, step.Name, exception);
    }

    internal static void ExecutionFinished(this ILogger<Bootstrapper> self, BootstrapStep step, TimeSpan duration) {
        ExecutionFinishedDelegate(self, step.Name, duration.Milliseconds, null /* exception */);
    }
}