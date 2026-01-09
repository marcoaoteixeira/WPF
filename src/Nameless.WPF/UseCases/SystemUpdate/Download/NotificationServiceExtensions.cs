using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

internal static class NotificationServiceExtensions {
    extension(INotificationService self) {
        internal Task NotifySuccessAsync(string filePath) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: string.Format(Strings.DownloadUpdateNotification_Success, filePath),
                type: NotificationType.Success
            ));
        }

        internal Task NotifyFailureAsync(string error) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: error,
                type: NotificationType.Error
            ));
        }

        internal Task NotifyStartingAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.DownloadUpdateNotification_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyWritingFileAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.DownloadUpdateNotification_WritingFile,
                type: NotificationType.Information
            ));
        }
    }
}
