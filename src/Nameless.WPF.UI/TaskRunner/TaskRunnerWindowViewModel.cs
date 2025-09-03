using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UI.Components;
using Nameless.WPF.UI.Mvvm;

namespace Nameless.WPF.UI.TaskRunner;

public partial class TaskRunnerWindowViewModel : ViewModel {
    private readonly INotificationService _notificationService;

    private CancellationTokenSource? _cts;
    private Action? _subscribe;
    private Action? _unsubscribe;
    private TaskRunnerDelegate? _delegate;

    [ObservableProperty]
    private string _title = Strings.TaskRunnerWindow_Default_Title;

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

    [RelayCommand]
    private async Task ExecuteAsync() {
        if (_delegate is null) { return; }

        ToggleStatus();

        try {
            _subscribe?.Invoke();

            await _delegate(GetCancellationTokenSource().Token);
        }
        catch (Exception ex) { Entries.Add(LoggerRichTextBoxEntry.Error(ex.Message)); }
        finally { _unsubscribe?.Invoke(); }

        ToggleStatus();
    }

    [RelayCommand]
    private Task CancelAsync() {
        return GetCancellationTokenSource().CancelAsync();
    }

    [RelayCommand]
    private Task CloseAsync(object? sender) {
        if (sender is not Window window) {
            throw new InvalidOperationException($"Parameter '{nameof(sender)}' must be a '{nameof(Window)}' type.");
        }

        Clean();

        window.Close();

        return Task.CompletedTask;
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

    private void ToggleStatus() {
        Running = !Running;
        Idle = !Idle;
    }

    private void Clean() {
        _cts?.Dispose();
        _subscribe = null;
        _unsubscribe = null;
        _delegate = null;
    }
}
