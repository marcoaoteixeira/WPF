namespace Nameless.WPF.Notifications;

public interface INotificationService {
    void Subscribe<TNotification>(object recipient, Action<object, TNotification> handler)
        where TNotification : class, INotification;

    void Unsubscribe<TNotification>(object recipient)
        where TNotification : class, INotification;

    Task PublishAsync<TNotification>(TNotification notification)
        where TNotification : class, INotification;
}