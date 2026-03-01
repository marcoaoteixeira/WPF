namespace Nameless.WPF.Notifications;

public interface IPushNotification {
    void Subscribe<TMessage>(object recipient, Action<object, TMessage> handler)
        where TMessage : PushNotificationMessage;

    void Unsubscribe<TMessage>(object recipient)
        where TMessage : PushNotificationMessage;

    Task PublishAsync<TMessage>(TMessage notification)
        where TMessage : PushNotificationMessage;
}