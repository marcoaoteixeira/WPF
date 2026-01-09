using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

internal static class NotificationServiceExtensions {
    extension(INotificationService self) {
        internal Task NotifySuccessAsync() {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: Strings.CheckForUpdateNotification_Success_CurrentVersionUpToDate,
                type: NotificationType.Success,
                newVersion: null
            ));
        }

        internal Task NotifySuccessAsync(string newVersion) {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: string.Format(Strings.CheckForUpdateNotification_Success_NewVersionAvailable, newVersion),
                type: NotificationType.Success,
                newVersion: newVersion
            ));
        }

        internal Task NotifyFailureAsync(string error) {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: error,
                type: NotificationType.Error,
                newVersion: null
            ));
        }

        internal Task NotifyStartingAsync() {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: Strings.CheckForUpdateRequestHandler_Starting,
                type: NotificationType.Information,
                newVersion: null
            ));
        }
    }
}
