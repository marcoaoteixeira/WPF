namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Represents a bootstrap step.
/// </summary>
public abstract class BootstrapStep {
    /// <summary>
    ///     Gets the name of the step
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    ///     Whether it should throw an exception on failure.
    /// </summary>
    public virtual bool ThrowsOnFailure => false;

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
    public abstract Task ExecuteAsync(CancellationToken cancellationToken);
}
