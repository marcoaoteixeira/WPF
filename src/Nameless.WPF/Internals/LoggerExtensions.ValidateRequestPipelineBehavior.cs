using Microsoft.Extensions.Logging;
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

    extension<TRequest, TResponse>(ILogger<ValidateRequestPipelineBehavior<TRequest, TResponse>> self) where TRequest : notnull {
        internal void ValidateRequestObjectFailure(ValidationResult result) {
            ValidateRequestObjectFailureDelegate(self, typeof(TRequest).GetPrettyName(), result, null /* exception */);
        }
    }
}
