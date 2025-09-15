namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Represents the <see cref="Bootstrapper"/>
///     configuration options.
/// </summary>
public class BootstrapperOptions {
    // We use a list here because the order of the registration
    // is important when resolving.
    private readonly List<Type> _steps = [];

    internal IReadOnlyList<Type> Steps => _steps;

    /// <summary>
    ///     Registers a step for the <see cref="Bootstrapper"/>.
    /// </summary>
    /// <typeparam name="TStep">
    ///     Type of the step.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="BootstrapperOptions"/> so other actions
    ///     can be chained.
    /// </returns>
    public BootstrapperOptions RegisterStep<TStep>()
        where TStep : BootstrapStep {
        if (!_steps.Contains(typeof(TStep))) {
            _steps.Add(typeof(TStep));
        }

        return this;
    }
}
