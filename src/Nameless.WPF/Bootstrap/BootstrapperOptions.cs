namespace Nameless.WPF.Bootstrap;
public class BootstrapperOptions {
    internal List<Type> Steps { get; } = [];

    public BootstrapperOptions RegisterStep<TStep>()
        where TStep : BootstrapStep {
        if (!Steps.Contains(typeof(TStep))) {
            Steps.Add(typeof(TStep));
        }

        return this;
    }
}
