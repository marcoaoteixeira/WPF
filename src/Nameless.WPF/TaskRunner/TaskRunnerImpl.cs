using Nameless.WPF.Windows;

namespace Nameless.WPF.TaskRunner;

public class TaskRunnerImpl : ITaskRunner {
    private readonly IWindowFactory _windowFactory;

    public TaskRunnerImpl(IWindowFactory windowFactory) {
        _windowFactory = Guard.Against.Null(windowFactory);
    }

    public TaskRunnerBuilder CreateBuilder() {
        return new TaskRunnerBuilder(_windowFactory);
    }
}