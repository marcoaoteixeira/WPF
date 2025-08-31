using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UI.Dialogs.TaskRunnerDialog;

/// <summary>
///     Action handler window.
/// </summary>
[ServiceLifetime(Lifetime = ServiceLifetime.Transient)]
public partial class TaskRunnerDialogWindow : ITaskRunnerDialogWindow {
    /// <inheritdoc />
    public TaskRunnerDialogWindowViewModel ViewModel { get; }

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="TaskRunnerDialogWindow"/> class.
    /// </summary>
    /// <param name="viewModel">
    ///     The view model.
    /// </param>
    public TaskRunnerDialogWindow(TaskRunnerDialogWindowViewModel viewModel) {
        ViewModel = Guard.Against.Null(viewModel);

        DataContext = ViewModel;

        InitializeComponent();
    }

    /// <inheritdoc />
    public ITaskRunnerDialogWindow SetTitle(string title) {
        ViewModel.Title = Guard.Against.NullOrWhiteSpace(title);

        return this;
    }

    /// <inheritdoc />
    public ITaskRunnerDialogWindow SetOwner(Window? owner) {
        CallOnDispatcher(() => Owner = owner ?? Application.Current.MainWindow);

        return this;
    }

    /// <inheritdoc />
    public ITaskRunnerDialogWindow SubscribeFor<TNotification>()
        where TNotification : class, INotification {
        ViewModel.SubscribeFor<TNotification>();

        return this;
    }

    /// <inheritdoc />
    public ITaskRunnerDialogWindow SetHandler(TaskRunnerAsyncHandler handler) {
        ViewModel.SetHandler(Guard.Against.Null(handler));

        return this;
    }

    /// <inheritdoc />
    public void Show(WindowStartupLocation startupLocation) {
        WindowStartupLocation = startupLocation;

        CallOnDispatcher(() => ShowDialog());
    }

    private void StartupHandler(object sender, RoutedEventArgs args) {
        Dispatcher.InvokeAsync(() => ViewModel.ExecuteCommand.ExecuteAsync(null));
    }

    private void CallOnDispatcher(Action action) {
        var dispatch = Dispatcher.CheckAccess();

        if (dispatch) { Dispatcher.Invoke(action); }
        else { action(); }
    }
}