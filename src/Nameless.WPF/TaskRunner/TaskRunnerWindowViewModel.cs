using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.WPF.Components;
using Nameless.WPF.Mvvm;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.TaskRunner;

public partial class TaskRunnerWindowViewModel : ViewModel {
    private readonly INotificationService _notificationService;

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
    /// <param name="notificationService">
    ///     The notification service.
    /// </param>
    public TaskRunnerWindowViewModel(INotificationService notificationService) {
        _notificationService = Guard.Against.Null(notificationService);
    }

    /// <summary>
    ///     Subscribes for a specific notification type.
    /// </summary>
    /// <typeparam name="TNotification">
    ///     The notification type.
    /// </typeparam>
    public void SubscribeFor<TNotification>()
        where TNotification : class, INotification {
        _subscribe += () => _notificationService.Subscribe<TNotification>(this, Update);
        _unsubscribe += () => _notificationService.Unsubscribe<TNotification>(this);
    }

    /// <summary>
    ///     Sets the async delegate to be executed.
    /// </summary>
    /// <param name="delegate">
    ///     The async delegate.
    /// </param>
    public void SetHandler(TaskRunnerDelegate @delegate) {
        _delegate = Guard.Against.Null(@delegate);
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

    private void Update<TNotification>(object sender, TNotification notification)
        where TNotification : class, INotification {
        var entry = notification.Type switch {
            NotificationType.Information => LoggerRichTextBoxEntry.Information(notification.Message),
            NotificationType.Error => LoggerRichTextBoxEntry.Error(notification.Message),
            NotificationType.Success => LoggerRichTextBoxEntry.Success(notification.Message),
            NotificationType.Warning => LoggerRichTextBoxEntry.Warning(notification.Message),
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
