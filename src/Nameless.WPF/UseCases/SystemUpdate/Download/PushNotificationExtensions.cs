using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

internal static class PushNotificationExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<DownloadUpdatePushNotificationMessage>(
                message: Strings.DownloadUpdateNotification_Starting
            );
        }

        internal Task NotifyWritingFileAsync() {
            return self.PublishInformationAsync<DownloadUpdatePushNotificationMessage>(
                message: Strings.DownloadUpdateNotification_WritingFile
            );
        }

        internal Task NotifySuccessAsync(string filePath) {
            return self.PublishSuccessAsync<DownloadUpdatePushNotificationMessage>(
                message: string.Format(Strings.DownloadUpdateNotification_Success, filePath)
            );
        }
        internal Task NotifyFailureAsync(string error) {
            return self.PublishErrorAsync<DownloadUpdatePushNotificationMessage>(
                message: error
            );
        }
    }
}
