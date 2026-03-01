namespace Nameless.WPF.Notifications;

public abstract record PushNotificationMessage {
    public string Message { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public Dictionary<string, object?> Metadata { get; init; } = [];

    public PushNotificationType Type { get; init; }
}