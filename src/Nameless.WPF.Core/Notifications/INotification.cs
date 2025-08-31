namespace Nameless.WPF.Notifications;

public interface INotification {
    string? Title { get; }

    string Message { get; }

    NotificationType Type { get; }
}