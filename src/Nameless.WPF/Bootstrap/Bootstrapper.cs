using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nameless.Null;

namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Default implementation of <see cref="IBootstrapper"/>.
/// </summary>
public class Bootstrapper : IBootstrapper {
    private readonly Step[] _steps;
    private readonly ILogger<Bootstrapper> _logger;

    private int _currentStep;

    private IProgress<BootstrapperProgressReport> Progress { get; set; } = NullProgress<BootstrapperProgressReport>.Instance;

    /// <inheritdoc />
    public int Steps => _steps.Length;

    /// <summary>
    ///     Initializes a new instance of <see cref="Bootstrapper"/> class.
    /// </summary>
    /// <param name="steps">
    ///     The bootstrap steps.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public Bootstrapper(IEnumerable<Step> steps, ILogger<Bootstrapper> logger) {
        _steps = [.. steps];
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken) {
        var sw = Stopwatch.StartNew();
        var progress = new Progress<StepProgressReport>(OnStepProgress);

        _currentStep = 1;

        foreach (var step in _steps) {
            sw.Reset();

            step.SetProgress(progress);

            _logger.StartingExecution(step);

            try { await step.ExecuteAsync(cancellationToken); }
            catch (Exception ex) {
                _logger.ExecutionFailure(step, ex);

                if (step.ThrowsOnFailure) {
                    throw;
                }
            }
            finally {
                _currentStep++;

                _logger.ExecutionFinished(step, sw.Elapsed);
            }
        }
    }

    public void SetProgress(IProgress<BootstrapperProgressReport> progress) {
        Progress = progress;
    }

    private void OnStepProgress(StepProgressReport report) {
        Progress.Report(new BootstrapperProgressReport(_currentStep, report.Title, report.Message));
    }
}