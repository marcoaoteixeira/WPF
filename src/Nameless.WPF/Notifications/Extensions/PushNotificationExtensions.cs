using Nameless.WPF.Helpers;
using Nameless.WPF.Notifications.Impl;

namespace Nameless.WPF.Notifications;

public static class PushNotificationExtensions {
    extension(IPushNotification self) {
        public Task PublishInformationAsync<TMessage>(string message, string? title = null, object? metadata = null)
            where TMessage : PushNotificationMessage, new() {
            return self.InnerPublishAsync<TMessage>(PushNotificationType.Information, message, title, metadata);
        }

        public Task PublishSuccessAsync<TMessage>(string message, string? title = null, object? metadata = null)
            where TMessage : PushNotificationMessage, new() {
            return self.InnerPublishAsync<TMessage>(PushNotificationType.Success, message, title, metadata);
        }

        public Task PublishWarningAsync<TMessage>(string message, string? title = null, object? metadata = null)
            where TMessage : PushNotificationMessage, new() {
            return self.InnerPublishAsync<TMessage>(PushNotificationType.Warning, message, title, metadata);
        }

        public Task PublishErrorAsync<TMessage>(string message, string? title = null, object? metadata = null)
            where TMessage : PushNotificationMessage, new() {
            return self.InnerPublishAsync<TMessage>(PushNotificationType.Error, message, title, metadata);
        }

        public Task SnackBarInformationAsync(string message, string? title = null, object? metadata = null) {
            return self.InnerPublishAsync<SnackBarPushNotificationMessage>(PushNotificationType.Information, message, title, metadata);
        }

        public Task SnackBarSuccessAsync(string message, string? title = null, object? metadata = null) {
            return self.InnerPublishAsync<SnackBarPushNotificationMessage>(PushNotificationType.Success, message, title, metadata);
        }

        public Task SnackBarWarningAsync(string message, string? title = null, object? metadata = null) {
            return self.InnerPublishAsync<SnackBarPushNotificationMessage>(PushNotificationType.Warning, message, title, metadata);
        }

        public Task SnackBarErrorAsync(string message, string? title = null, object? metadata = null) {
            return self.InnerPublishAsync<SnackBarPushNotificationMessage>(PushNotificationType.Error, message, title, metadata);
        }

        private Task InnerPublishAsync<TMessage>(PushNotificationType type, string message, string? title = null, object? metadata = null)
            where TMessage : PushNotificationMessage, new() {
            
            var obj = new TMessage {
                Message = message,
                Title = title ?? typeof(TMessage).Name,
                Metadata = ObjectHelper.Transform(metadata),
                Type = type
            };

            return self.PublishAsync(obj);
        }
    }
}
