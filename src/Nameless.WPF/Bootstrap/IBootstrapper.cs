namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Responsible for execute the bootstrap steps.
/// </summary>
public interface IBootstrapper {
    /// <summary>
    ///     Gets the total number of steps to be executed.
    /// </summary>
    int Steps { get; }

    /// <summary>
    ///     Executes all necessary steps for the bootstrap to occur.
    /// </summary>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous action
    ///     execution.
    /// </returns>
    Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Sets the progress reporter.
    /// </summary>
    /// <param name="progress">
    ///     The progress reporter.
    /// </param>
    void SetProgress(IProgress<BootstrapperProgressReport> progress);
}