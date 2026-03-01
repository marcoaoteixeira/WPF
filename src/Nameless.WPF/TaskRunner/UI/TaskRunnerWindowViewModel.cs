using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.WPF.Controls;
using Nameless.WPF.Mvvm;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.TaskRunner.UI;

public partial class TaskRunnerWindowViewModel : ViewModel {
    private readonly IPushNotification _pushNotification;

    private CancellationTokenSource? _cts;
    private Action? _subscribe;
    private Action? _unsubscribe;
    private TaskRunnerDelegate? _delegate;

    [ObservableProperty]
    private string _title = Strings.TaskRunnerWindow_Title;

    [ObservableProperty]
    private bool _running;

    [ObservableProperty]
    private bool _idle = true;

    [ObservableProperty]
    private ObservableCollection<LoggerRichTextBoxEntry> _entries = [];

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="TaskRunnerWindowViewModel"/> class.
    /// </summary>
    /// <param name="pushNotification">
    ///     The notification service.
    /// </param>
    public TaskRunnerWindowViewModel(IPushNotification pushNotification) {
        _pushNotification = pushNotification;
    }

    /// <summary>
    ///     Subscribes for a specific notification type.
    /// </summary>
    /// <typeparam name="TPushNotificationMessage">
    ///     The notification type.
    /// </typeparam>
    public void SubscribeFor<TPushNotificationMessage>()
        where TPushNotificationMessage : PushNotificationMessage {
        _subscribe += () => _pushNotification.Subscribe<TPushNotificationMessage>(this, Update);
        _unsubscribe += () => _pushNotification.Unsubscribe<TPushNotificationMessage>(this);
    }

    /// <summary>
    ///     Sets the async delegate to be executed.
    /// </summary>
    /// <param name="delegate">
    ///     The async delegate.
    /// </param>
    public void SetHandler(TaskRunnerDelegate @delegate) {
        _delegate = @delegate;
    }

    /// <summary>
    ///     Cleans up all resources used by the task runner.
    /// </summary>
    public void CleanUp() {
        if (Running) {
            _cts?.Cancel(throwOnFirstException: false);
        }

        _cts?.Dispose();
        _cts = null;
        _subscribe = null;
        _unsubscribe = null;
        _delegate = null;
    }

    [RelayCommand]
    private async Task ExecuteAsync() {
        if (_delegate is null) { return; }

        await ToggleRunningAsync();

        _subscribe?.Invoke();

        try { await _delegate(GetCancellationTokenSource().Token); }
        catch (Exception ex) { Entries.Add(LoggerRichTextBoxEntry.Error(ex.Message)); }

        _unsubscribe?.Invoke();

        await ToggleRunningAsync();
    }

    [RelayCommand]
    private Task CancelAsync() {
        return GetCancellationTokenSource().CancelAsync();
    }

    private CancellationTokenSource GetCancellationTokenSource() {
        return _cts ??= new CancellationTokenSource();
    }

    private void Update<TPushNotificationMessage>(object sender, TPushNotificationMessage notification)
        where TPushNotificationMessage : PushNotificationMessage {
        var entry = notification.Type switch {
            PushNotificationType.Success => LoggerRichTextBoxEntry.Success(notification.Message),
            PushNotificationType.Warning => LoggerRichTextBoxEntry.Warning(notification.Message),
            PushNotificationType.Error => LoggerRichTextBoxEntry.Error(notification.Message),
            _ => LoggerRichTextBoxEntry.Information(notification.Message)
        };

        Entries.Add(entry);
    }

    private async Task ToggleRunningAsync() {
        // Small delay so any UI control can be
        // rendered properly.
        await Task.Delay(200);

        Running = !Running;
        Idle = !Idle;
    }
}
