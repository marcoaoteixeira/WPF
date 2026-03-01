using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

internal static class PushNotificationExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<PerformLuceneBackupPushNotificationMessage>(
                message: Strings.PerformLuceneBackup_PushNotificationMessage_Starting
            );
        }

        internal Task NotifyFailureAsync(string error) {
            return self.PublishErrorAsync<PerformLuceneBackupPushNotificationMessage>(
                message: string.Format(Strings.PerformLuceneBackup_PushNotificationMessage_Failure, error)
            );
        }

        internal Task NotifySuccessAsync(string backupFilePath) {
            return self.PublishSuccessAsync<PerformLuceneBackupPushNotificationMessage>(
                message: string.Format(Strings.PerformLuceneBackup_PushNotificationMessage_Success, backupFilePath)
            );
        }
    }
}
