using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

internal static class PushNotificationExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<CheckForUpdatePushNotificationMessage>(
                message: Strings.CheckForUpdateRequestHandler_Starting
            );
        }

        internal Task NotifyFailureAsync(string error) {
            return self.PublishErrorAsync<CheckForUpdatePushNotificationMessage>(
                message: error
            );
        }

        internal Task NotifySuccessAsync() {
            return self.PublishSuccessAsync<CheckForUpdatePushNotificationMessage>(
                message: Strings.CheckForUpdateNotification_Success_CurrentVersionUpToDate
            );
        }

        internal Task NotifySuccessAsync(string newVersion) {
            return self.PublishSuccessAsync<CheckForUpdatePushNotificationMessage>(
                message: string.Format(Strings.CheckForUpdateNotification_Success_NewVersionAvailable, newVersion)
            );
        }
    }
}
