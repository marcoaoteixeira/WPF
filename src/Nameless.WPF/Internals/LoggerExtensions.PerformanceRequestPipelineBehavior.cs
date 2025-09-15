using Microsoft.Extensions.Logging;
using Nameless.Mediator.Requests;
using Nameless.WPF.Behaviors;

namespace Nameless.WPF.Internals;

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

    internal static void StartPerformanceMonitor<TRequest, TResponse>(this ILogger<PerformanceRequestPipelineBehavior<TRequest, TResponse>> self)
        where TRequest : IRequest<TResponse> {
        StartPerformanceMonitorDelegate(self, typeof(TRequest).GetPrettyName(), typeof(TResponse).GetPrettyName(), null /* exception */);
    }

    internal static void FinishPerformanceMonitor<TRequest, TResponse>(this ILogger<PerformanceRequestPipelineBehavior<TRequest, TResponse>> self, long elapsedMilliseconds)
        where TRequest : IRequest<TResponse> {
        FinishPerformanceMonitorDelegate(self, typeof(TRequest).GetPrettyName(), typeof(TResponse).GetPrettyName(), elapsedMilliseconds, null /* exception */);
    }
}
