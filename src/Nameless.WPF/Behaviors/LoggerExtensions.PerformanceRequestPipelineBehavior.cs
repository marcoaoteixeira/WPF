using Microsoft.Extensions.Logging;

namespace Nameless.WPF.Behaviors;

public static class PerformanceRequestPipelineBehaviorLoggerExtensions {
    private static readonly Action<ILogger, string, string, Exception?> StartPerformanceMonitorDelegate
        = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Debug,
            eventId: default,
            formatString: "Executing request handler 'IRequestHandler<{RequestType}, {ResponseType}>'..."
        );

    private static readonly Action<ILogger, string, string, long, Exception?> FinishPerformanceMonitorDelegate
        = LoggerMessage.Define<string, string, long>(
            logLevel: LogLevel.Debug,
            eventId: default,
            formatString: "Execution of request handler 'IRequestHandler<{RequestType}, {ResponseType}>' took {ElapsedMilliseconds}ms."
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
}
