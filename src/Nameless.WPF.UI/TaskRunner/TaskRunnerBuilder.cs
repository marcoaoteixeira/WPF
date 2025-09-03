﻿using System.Windows;
using Nameless.WPF.Notifications;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.TaskRunner;

public class TaskRunnerBuilder : ITaskRunnerBuilder {
    private readonly IWindowFactory _windowFactory;
    private readonly Lazy<ITaskRunnerWindow> _taskRunnerWindow;

    private ITaskRunnerWindow Window => _taskRunnerWindow.Value;

    public TaskRunnerBuilder(IWindowFactory windowFactory) {
        _windowFactory = Guard.Against.Null(windowFactory);
        _taskRunnerWindow = new Lazy<ITaskRunnerWindow>(CreateTaskRunnerWindow);
    }

    public ITaskRunnerBuilder SetName(string name) {
        Window.SetName(name);

        return this;
    }

    public ITaskRunnerBuilder SetDelegate(TaskRunnerDelegate @delegate) {
        Window.SetDelegate(@delegate);

        return this;
    }

    public ITaskRunnerBuilder SubscribeFor<TNotification>()
        where TNotification : class, INotification {
        Window.SubscribeFor<TNotification>();

        return this;
    }

    public Task RunAsync() {
        Window.Show(WindowStartupLocation.CenterScreen);

        return Task.CompletedTask;
    }

    private ITaskRunnerWindow CreateTaskRunnerWindow() {
        if (!_windowFactory.TryCreate<ITaskRunnerWindow>(out var output)) {
            throw new InvalidOperationException($"Couldn't create {nameof(ITaskRunnerWindow)} instance.");
        }

        output.SetOwner(Application.Current.MainWindow);

        return output;
    }
}