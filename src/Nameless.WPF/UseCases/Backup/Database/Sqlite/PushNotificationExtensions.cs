using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

internal static class PushNotificationExtensions {
    extension(IPushNotification self) {
        internal Task NotifyStartingAsync() {
            return self.PublishInformationAsync<PerformSqliteBackupPushNotificationMessage>(
                message: Strings.PerformSqliteBackup_PushNotificationMessage_Starting
            );
        }

        internal Task NotifyFailureAsync(string error) {
            return self.PublishErrorAsync<PerformSqliteBackupPushNotificationMessage>(
                message: string.Format(Strings.PerformSqliteBackup_PushNotificationMessage_Failure, error)
            );
        }

        internal Task NotifySuccessAsync(string backupFilePath) {
            return self.PublishSuccessAsync<PerformSqliteBackupPushNotificationMessage>(
                message: string.Format(Strings.PerformSqliteBackup_PushNotificationMessage_Success, backupFilePath)
            );
        }
    }
}
