using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Default implementation of <see cref="IBootstrapper"/>.
/// </summary>
public class Bootstrapper : IBootstrapper {
    private readonly IEnumerable<BootstrapStep> _steps;
    private readonly ILogger<Bootstrapper> _logger;

    /// <summary>
    ///     Initializes a new instance of <see cref="Bootstrapper"/> class.
    /// </summary>
    /// <param name="steps">
    ///     The bootstrap steps.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public Bootstrapper(IEnumerable<BootstrapStep> steps, ILogger<Bootstrapper> logger) {
        _steps = Guard.Against.Null(steps);
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken) {
        var sw = Stopwatch.StartNew();

        foreach (var step in _steps.Reverse()) {
            sw.Reset();

            _logger.StartingExecution(step);
            try {
                await step.ExecuteAsync(cancellationToken);
            }
            catch (Exception ex) {
                _logger.ExecutionFailure(step, ex);

                if (step.ThrowsOnFailure) {
                    throw;
                }
            }
            finally { _logger.ExecutionFinished(step, sw.Elapsed); }
        }
    }
}