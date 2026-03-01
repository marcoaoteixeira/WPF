using Microsoft.Extensions.Logging;
using Nameless.Validation;
<<<<<<< Updated upstream

namespace Nameless.WPF.Behaviors;

// Root object used to nest other logger extensions.
// You can see it in the "Solution Explorer" using
// the nesting setting "Web"
=======

namespace Nameless.WPF.Behaviors;

>>>>>>> Stashed changes
internal static class LoggerExtensions {
    private static readonly Action<ILogger, string, string, Exception?> StartPerformanceMonitorDelegate
        = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Debug,
            eventId: default,
<<<<<<< Updated upstream
            formatString: "Executing request handler 'IRequestHandler<{RequestType}, {ResponseType}>'..."
=======
            formatString: "[PERFORMANCE] Starting 'IRequestHandler<{RequestType}, {ResponseType}>'..."
>>>>>>> Stashed changes
        );

    private static readonly Action<ILogger, string, string, long, Exception?> FinishPerformanceMonitorDelegate
        = LoggerMessage.Define<string, string, long>(
            logLevel: LogLevel.Debug,
            eventId: default,
<<<<<<< Updated upstream
            formatString: "Execution of request handler 'IRequestHandler<{RequestType}, {ResponseType}>' took {ElapsedMilliseconds}ms."
=======
            formatString: "[PERFORMANCE] 'IRequestHandler<{RequestType}, {ResponseType}>' execution finished in {ElapsedMilliseconds}ms."
>>>>>>> Stashed changes
        );

    private static readonly Action<ILogger, string, ValidationResult, Exception?> ValidateRequestObjectFailureDelegate
        = LoggerMessage.Define<string, ValidationResult>(
            logLevel: LogLevel.Warning,
            eventId: default,
<<<<<<< Updated upstream
            formatString: "Request object '{RequestType}' is invalid: {@ValidationResult}"
=======
            formatString: "[VALIDATION] Request object '{RequestType}' is invalid: {@ValidationResult}"
>>>>>>> Stashed changes
        );

    extension<TRequest, TResponse>(ILogger<PerformanceRequestPipelineBehavior<TRequest, TResponse>> self)
        where TRequest : notnull {
        internal void StartPerformanceMonitor() {
            StartPerformanceMonitorDelegate(self, typeof(TRequest).GetPrettyName(), typeof(TResponse).GetPrettyName(), null /* exception */);
        }

        internal void FinishPerformanceMonitor(long elapsedMilliseconds) {
            FinishPerformanceMonitorDelegate(self, typeof(TRequest).GetPrettyName(), typeof(TResponse).GetPrettyName(), elapsedMilliseconds, null /* exception */);
        }
    }

    extension<TRequest, TResponse>(ILogger<ValidateRequestPipelineBehavior<TRequest, TResponse>> self) where TRequest : notnull {
        internal void ValidateRequestObjectFailure(ValidationResult result) {
            ValidateRequestObjectFailureDelegate(self, typeof(TRequest).GetPrettyName(), result, null /* exception */);
        }
    }
}
