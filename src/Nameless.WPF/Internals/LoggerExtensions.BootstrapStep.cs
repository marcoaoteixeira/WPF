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

    extension(ILogger<Bootstrapper> self) {
        internal void StartingExecution(Step step) {
            StartingExecutionDelegate(self, step.Name, null /* exception */);
        }

        internal void ExecutionFailure(Step step, Exception exception) {
            ExecutionFailureDelegate(self, step.Name, exception);
        }

        internal void ExecutionFinished(Step step, TimeSpan duration) {
            ExecutionFinishedDelegate(self, step.Name, duration.Milliseconds, null /* exception */);
        }
    }
}