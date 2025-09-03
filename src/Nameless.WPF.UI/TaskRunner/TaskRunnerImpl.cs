using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.TaskRunner;

public class TaskRunnerImpl : ITaskRunner {
    private readonly IWindowFactory _windowFactory;

    public TaskRunnerImpl(IWindowFactory windowFactory) {
        _windowFactory = Guard.Against.Null(windowFactory);
    }

    public TaskRunnerBuilder CreateBuilder() {
        return new TaskRunnerBuilder(_windowFactory);
    }
}