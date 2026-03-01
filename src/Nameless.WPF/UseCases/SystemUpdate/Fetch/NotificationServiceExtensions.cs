using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;
using Nameless.WPF.UseCases.SystemUpdate.Download;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

internal static class NotificationServiceExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<DownloadUpdatePushNotificationMessage>(
                message: Strings.FetchNewVersionInformationNotification_Starting
            );
        }

        internal Task NotifyFailureAsync(string version, string error) {
            return self.PublishErrorAsync<DownloadUpdatePushNotificationMessage>(
                message: string.Format(Strings.FetchNewVersionInformationNotification_Failure, version, error)
            );
        }

        internal Task NotifyNotFoundAsync() {
            return self.PublishErrorAsync<DownloadUpdatePushNotificationMessage>(
                message: Strings.FetchNewVersionInformationNotification_NotFound
            );
        }

        internal Task NotifySuccessAsync() {
            return self.PublishSuccessAsync<DownloadUpdatePushNotificationMessage>(
                message: Strings.FetchNewVersionInformationNotification_Success
            );
        }
    }
}
