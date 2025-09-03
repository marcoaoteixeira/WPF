using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nameless.Mediator.Requests;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Behaviors;

public class PerformanceRequestPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    private readonly ILogger<PerformanceRequestPipelineBehavior<TRequest, TResponse>> _logger;

    public PerformanceRequestPipelineBehavior(ILogger<PerformanceRequestPipelineBehavior<TRequest, TResponse>> logger) {
        _logger = Guard.Against.Null(logger);
    }

    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        var sw = Stopwatch.StartNew();

        _logger.StartPerformanceMonitor();

        var response = await next(cancellationToken);

        _logger.FinishPerformanceMonitor(sw.ElapsedMilliseconds);

        return response;
    }
}
