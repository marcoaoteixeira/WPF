using Nameless.WPF.Notifications;

namespace Nameless.WPF.UI.TaskRunner;

public interface ITaskRunnerBuilder {
    ITaskRunnerBuilder SetName(string name);

    ITaskRunnerBuilder SetDelegate(TaskRunnerDelegate @delegate);

    ITaskRunnerBuilder SubscribeFor<TNotification>()
        where TNotification : class, INotification;

    Task RunAsync();
}