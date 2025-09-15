using System.Windows;
using Nameless.WPF.Mvvm;
using Nameless.WPF.Notifications;
using Nameless.WPF.Windows;

namespace Nameless.WPF.TaskRunner;

public interface ITaskRunnerWindow : IWindow, IHasViewModel<TaskRunnerWindowViewModel> {
    /// <summary>
    ///     Sets the name of the window.
    /// </summary>
    /// <param name="name">
    ///     The name.
    /// </param>
    /// <returns>
    ///     The current <see cref="ITaskRunnerWindow"/> instance so
    ///     other actions can be chained.
    /// </returns>
    ITaskRunnerWindow SetName(string name);

    /// <summary>
    ///     Sets the 
    /// </summary>
    /// <param name="delegate">
    ///     The delegate.
    /// </param>
    ITaskRunnerWindow SetDelegate(TaskRunnerDelegate @delegate);

    /// <summary>
    ///     Subscribes for notification.
    /// </summary>
    /// <typeparam name="TNotification">
    ///     Type of the notification.
    /// </typeparam>
    ITaskRunnerWindow SubscribeFor<TNotification>()
        where TNotification : class, INotification;

    /// <summary>
    ///     Sets the dialog owner.
    /// </summary>
    /// <param name="owner">
    ///     The dialog owner.
    /// </param>
    /// <returns>
    ///     The current <see cref="ITaskRunnerWindow"/> instance so
    ///     other actions can be chained.
    /// </returns>
    ITaskRunnerWindow SetOwner(Window? owner);
}