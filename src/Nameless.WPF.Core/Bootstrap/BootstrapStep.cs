using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Represents a bootstrap step.
/// </summary>
public abstract class BootstrapStep {
    private readonly ILogger<BootstrapStep> _logger;

    /// <summary>
    ///     Gets the name of the step
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    ///     Gets the execution order of the step.
    /// </summary>
    public virtual int Order => 1;

    /// <summary>
    ///     Whether it should throw an exception on failure.
    /// </summary>
    public virtual bool ThrowsOnFailure => true;

    /// <summary>
    ///     Whether it should be skipped from the execution.
    /// </summary>
    public virtual bool Skip => false;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="BootstrapStep"/> class.
    /// </summary>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    protected BootstrapStep(ILogger<BootstrapStep> logger) {
        _logger = Guard.Against.Null(logger);
    }

    /// <summary>
    ///     Executes the step asynchronously.
    /// </summary>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous action
    ///     execution.
    /// </returns>
    public async Task ExecuteAsync(CancellationToken cancellationToken) {
        var sw = Stopwatch.StartNew();

        _logger.StartingExecution(this);

        try {
            await InnerExecuteAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (Exception ex) {
            _logger.ExecutionFailure(this, ex);

            if (ThrowsOnFailure) { throw; }
        }
        finally { _logger.ExecutionFinished(this, sw.Elapsed); }
    }

    /// <summary>
    ///     Executes the step.
    /// </summary>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous action
    ///     execution.
    /// </returns>
    protected abstract Task InnerExecuteAsync(CancellationToken cancellationToken);
}
