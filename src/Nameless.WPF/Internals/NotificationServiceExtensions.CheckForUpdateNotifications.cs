using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UseCases.SystemUpdate.Check;

namespace Nameless.WPF.Internals;

internal static class NotificationServiceExtensionsCheckForUpdateNotifications {
    extension(INotificationService self) {
        internal Task CheckForUpdateSuccessAsync() {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: Strings.CheckForUpdateNotification_Success_CurrentVersionUpToDate,
                type: NotificationType.Success,
                newVersion: null
            ));
        }

        internal Task CheckForUpdateSuccessAsync(string newVersion) {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: string.Format(Strings.CheckForUpdateNotification_Success_NewVersionAvailable, newVersion),
                type: NotificationType.Success,
                newVersion: newVersion
            ));
        }

        internal Task CheckForUpdateFailureAsync(string error) {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: error,
                type: NotificationType.Error,
                newVersion: null
            ));
        }

        internal Task CheckForUpdateStartingAsync() {
            return self.PublishAsync(new CheckForUpdateNotification(
                message: Strings.CheckForUpdateRequestHandler_Starting,
                type: NotificationType.Information,
                newVersion: null
            ));
        }
    }
}
