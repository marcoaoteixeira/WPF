using Microsoft.Extensions.Logging;
using Nameless.Mediator.Requests;
using Nameless.Validation;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Behaviors;

/// <summary>
///     A request pipeline behavior that validates the request object
///     before it reaches the actual request handler.
/// </summary>
/// <typeparam name="TRequest">
///     Type of the request.
/// </typeparam>
/// <typeparam name="TResponse">
///     Type of the response.
/// </typeparam>
public class ValidateRequestPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : class {
    private readonly IValidationService _validationService;
    private readonly ILogger<ValidateRequestPipelineBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="ValidateRequestPipelineBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="validationService">
    ///     The validation service.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public ValidateRequestPipelineBehavior(IValidationService validationService, ILogger<ValidateRequestPipelineBehavior<TRequest, TResponse>> logger) {
        _validationService = Guard.Against.Null(validationService);
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.Null(next);

        var result = await _validationService.ValidateAsync(request, cancellationToken)
                                             .SuppressContext();

        if (result.Succeeded) {
            return await next(cancellationToken).SuppressContext();
        }

        _logger.ValidateRequestObjectFailure(result);

        throw new ValidationException(result);
    }
}