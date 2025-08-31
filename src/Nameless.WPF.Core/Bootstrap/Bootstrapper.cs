namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Default implementation of <see cref="IBootstrapper"/>.
/// </summary>
public class Bootstrapper : IBootstrapper {
    private readonly IEnumerable<BootstrapStep> _steps;

    /// <summary>
    ///     Initializes a new instance of <see cref="Bootstrapper"/> class.
    /// </summary>
    /// <param name="steps">
    ///     The bootstrap steps.
    /// </param>
    public Bootstrapper(IEnumerable<BootstrapStep> steps) {
        _steps = Guard.Against.Null(steps);
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken) {
        var steps = _steps.Where(step => !step.Skip)
                          .OrderBy(step => step.Order);

        foreach (var step in steps) {
            await step.ExecuteAsync(cancellationToken);
        }
    }
}