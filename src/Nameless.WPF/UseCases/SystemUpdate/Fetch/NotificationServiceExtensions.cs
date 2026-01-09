using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UseCases.SystemUpdate.Download;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

internal static class NotificationServiceExtensions {
    extension(INotificationService self) {
        internal Task NotifySuccessAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_Success,
                type: NotificationType.Success
            ));
        }

        internal Task NotifyFailureAsync(string version, string error) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: string.Format(Strings.FetchNewVersionInformationNotification_Failure, version, error),
                type: NotificationType.Error
            ));
        }

        internal Task NotifyStartingAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyNotFoundAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.FetchNewVersionInformationNotification_NotFound,
                type: NotificationType.Error
            ));
        }
    }
}
