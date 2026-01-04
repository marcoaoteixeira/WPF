using Nameless.WPF.Client.Sqlite.Resources;
using Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.Internals;

// Root object used to nest other logger extensions.
// You can see it in the "Solution Explorer" using
// the nesting setting "Web"
internal readonly record struct NotificationServiceExtensions;

internal static class NotificationServiceExtensionsPerformDatabaseBackupNotifications {
    extension(INotificationService self) {
        internal Task PerformDatabaseBackup_SuccessAsync(string backupFilePath) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_Success, backupFilePath),
                type: NotificationType.Success
            ));
        }
        
        internal Task PerformDatabaseBackup_DatabaseBackup_StartingAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_BackupDatabase_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task PerformDatabaseBackup_DatabaseBackup_FinishAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_BackupDatabase_Finish,
                type: NotificationType.Information
            ));
        }

        internal Task PerformDatabaseBackup_DatabaseBackup_FailureAsync(string error) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_BackupDatabase_Failure, error),
                type: NotificationType.Error
            ));
        }

        internal Task PerformDatabaseBackup_PrepareBackupFile_StartingAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Starting,
                type: NotificationType.Information
            ));
        }

        internal Task PerformDatabaseBackup_PrepareBackupFile_FinishAsync() {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Finish,
                type: NotificationType.Information
            ));
        }

        internal Task PerformDatabaseBackup_PrepareBackupFile_FailureAsync(string error) {
            return self.PublishAsync(new PerformDatabaseBackupNotification(
                message: string.Format(Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Failure, error),
                type: NotificationType.Error
            ));
        }
    }
}