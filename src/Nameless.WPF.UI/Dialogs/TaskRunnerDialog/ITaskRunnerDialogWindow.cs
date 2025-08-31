using System.Windows;
using Nameless.WPF.Notifications;
using Nameless.WPF.UI.Mvvm;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.Dialogs.TaskRunnerDialog;

public interface ITaskRunnerDialogWindow : IWindow, IHasViewModel<TaskRunnerDialogWindowViewModel> {
    /// <summary>
    ///     Sets the title of the window.
    /// </summary>
    /// <param name="title">
    ///     The title.
    /// </param>
    /// <returns>
    ///     The current <see cref="ITaskRunnerDialogWindow"/> instance so
    ///     other actions can be chained.
    /// </returns>
    ITaskRunnerDialogWindow SetTitle(string title);

    /// <summary>
    ///     Sets the dialog owner.
    /// </summary>
    /// <param name="owner">
    ///     The dialog owner.
    /// </param>
    /// <returns>
    ///     The current <see cref="ITaskRunnerDialogWindow"/> instance so
    ///     other actions can be chained.
    /// </returns>
    ITaskRunnerDialogWindow SetOwner(Window? owner);

    /// <summary>
    ///     Subscribes for notification.
    /// </summary>
    /// <typeparam name="TNotification">
    ///     Type of the notification.
    /// </typeparam>
    ITaskRunnerDialogWindow SubscribeFor<TNotification>()
        where TNotification : class, INotification;

    /// <summary>
    ///     Sets the 
    /// </summary>
    /// <param name="handler">
    ///     The request.
    /// </param>
    ITaskRunnerDialogWindow SetHandler(TaskRunnerAsyncHandler handler);
}