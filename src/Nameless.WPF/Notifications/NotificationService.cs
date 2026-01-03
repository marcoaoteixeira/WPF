using CommunityToolkit.Mvvm.Messaging;

namespace Nameless.WPF.Notifications;

public class NotificationService : INotificationService {
    private readonly IMessenger _messenger;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="messenger">
    ///     The messenger.
    /// </param>
    public NotificationService(IMessenger messenger) {
        _messenger = messenger;
    }

    public void Subscribe<TMessage>(object recipient, Action<object, TMessage> handler)
        where TMessage : class, INotification {
        if (_messenger.IsRegistered<TMessage>(recipient)) {
            return;
        }

        _messenger.Register<TMessage>(
            recipient,
            handler: (destinatary, message) => handler(destinatary, message)
        );
    }

    public void Unsubscribe<TMessage>(object recipient)
        where TMessage : class, INotification {
        _messenger.Unregister<TMessage>(recipient);
    }

    public Task PublishAsync<TMessage>(TMessage notification)
        where TMessage : class, INotification {
        _messenger.Send(notification);

        return Task.CompletedTask;
    }
}