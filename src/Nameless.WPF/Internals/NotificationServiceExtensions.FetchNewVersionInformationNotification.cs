using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UseCases.SystemUpdate.Download;

namespace Nameless.WPF.Internals;

internal static class NotificationServiceExtensionsFetchNewVersionInformationNotifications {
    extension(INotificationService self) {
        internal Task FetchNewVersionInformationSuccessAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_Success,
                type: NotificationType.Success
            ));
        }

        internal Task FetchNewVersionInformationFailureAsync(string version, string error) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: string.Format(Strings.FetchNewVersionInformationNotification_Failure, version, error),
                type: NotificationType.Error
            ));
        }

        internal Task FetchNewVersionInformationNotFoundAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_NotFound,
                type: NotificationType.Error
            ));
        }

        internal Task FetchNewVersionInformationStartingAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_Starting,
                type: NotificationType.Information
            ));
        }
    }
}
