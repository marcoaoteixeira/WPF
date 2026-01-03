using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UseCases.SystemUpdate.Download;

namespace Nameless.WPF.Internals;

internal static class NotificationServiceExtensionsDownloadUpdateNotification {
    extension(INotificationService self) {
        internal Task DownloadUpdateSuccessAsync(string filePath) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: string.Format(Strings.DownloadUpdateNotification_Success, filePath),
                type: NotificationType.Success
            ));
        }

        internal Task DownloadUpdateFailureAsync(string error) {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: error,
                type: NotificationType.Error
            ));
        }

        internal Task DownloadUpdateStartingAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.DownloadUpdateNotification_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task DownloadUpdateWritingFileAsync() {
            return self.PublishAsync(new DownloadUpdateNotification(
                message: Strings.DownloadUpdateNotification_WritingFile,
                type: NotificationType.Information
            ));
        }
    }
}
