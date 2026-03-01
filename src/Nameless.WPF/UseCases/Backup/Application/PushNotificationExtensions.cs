using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Application;

internal static class PushNotificationExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<PerformApplicationBackupPushNotificationMessage>(
                message: Strings.PerformApplicationBackup_PushNotificationMessage_Starting
            );
        }

        internal Task NotifyFailureAsync() {
            return self.PublishErrorAsync<PerformApplicationBackupPushNotificationMessage>(
                message: Strings.PerformApplicationBackup_PushNotificationMessage_Failure
            );
        }

        internal Task NotifyCleanUpAsync() {
            return self.PublishInformationAsync<PerformApplicationBackupPushNotificationMessage>(
                message: Strings.PerformApplicationBackup_PushNotificationMessage_CleanUp
            );
        }

        internal Task NotifySuccessAsync(string backupFilePath) {
            return self.PublishSuccessAsync<PerformApplicationBackupPushNotificationMessage>(
                message: string.Format(Strings.PerformApplicationBackup_PushNotificationMessage_Success, backupFilePath)
            );
        }
    }
}