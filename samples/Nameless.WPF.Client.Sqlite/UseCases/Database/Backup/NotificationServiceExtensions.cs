using Nameless.WPF.Client.Sqlite.Resources;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

internal static class NotificationServiceExtensions {
    extension(INotificationService self) {
        internal Task NotifySuccessAsync(string backupFilePath) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_Success, backupFilePath),
                type: NotificationType.Success
            ));
        }
        
        internal Task NotifyDatabaseBackupStartingAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_BackupDatabase_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyDatabaseBackupFinishAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_BackupDatabase_Finish,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyDatabaseBackupFailureAsync(string error) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_BackupDatabase_Failure, error),
                type: NotificationType.Error
            ));
        }

        internal Task NotifyPrepareBackupFileStartingAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyPrepareBackupFileFinishAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Finish,
                type: NotificationType.Information
            ));
        }

        internal Task NotifyPrepareBackupFileFailureAsync(string error) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Failure, error),
                type: NotificationType.Error
            ));
        }
    }
}