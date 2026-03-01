using Nameless.WPF.Notifications;

namespace Nameless.WPF.TaskRunner;

public interface ITaskRunnerBuilder {
    ITaskRunnerBuilder SetName(string name);

    ITaskRunnerBuilder SetDelegate(TaskRunnerDelegate @delegate);

    ITaskRunnerBuilder SubscribeFor<TPushNotificationMessage>()
        where TPushNotificationMessage : PushNotificationMessage;

    Task RunAsync();
}