using Microsoft.Extensions.Logging;
using Nameless.Mediator.Requests;
using Nameless.Validation;
using Nameless.WPF.Behaviors;

namespace Nameless.WPF.Internals;

public static class ValidateRequestPipelineBehaviorLoggerExtensions {
    private static readonly Action<ILogger, string, ValidationResult, Exception?> ValidateRequestObjectFailureDelegate
        = LoggerMessage.Define<string, ValidationResult>(
            logLevel: LogLevel.Warning,
            eventId: default,
            formatString: "Request object '{RequestType}' is invalid: {@ValidationResult}"
        );

    internal static void ValidateRequestObjectFailure<TRequest, TResponse>(this ILogger<ValidateRequestPipelineBehavior<TRequest, TResponse>> self, ValidationResult result)
        where TRequest : IRequest<TResponse> {
        ValidateRequestObjectFailureDelegate(self, typeof(TRequest).GetPrettyName(), result, null /* exception */);
    }
}
