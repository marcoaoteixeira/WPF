using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Database.Backup;

public sealed record PerformDatabaseBackupNotification : INotification {
    public string Title => Strings.PerformDatabaseBackupNotification_Title;
    public string Message { get; }
    public NotificationType Type { get; }

    private PerformDatabaseBackupNotification(string message, NotificationType type = NotificationType.Information) {
        Message = message;
        Type = type;
    }

    public static PerformDatabaseBackupNotification Success(string backupFilePath) {
        return new PerformDatabaseBackupNotification(
            message: string.Format(Strings.PerformDatabaseBackupNotification_Success, backupFilePath),
            type: NotificationType.Success
        );
    }

    public static PerformDatabaseBackupNotification BackupDatabaseStarting() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_ExecuteBackup_Starting,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification BackupDatabaseFailure(string failure) {
        return new PerformDatabaseBackupNotification(
            message: string.Format(Strings.PerformDatabaseBackupNotification_ExecuteBackup_Failure, failure),
            type: NotificationType.Error
        );
    }

    public static PerformDatabaseBackupNotification BackupDatabaseFinish() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_ExecuteBackup_Finish,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileStarting() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Starting,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileFailure(string failure) {
        return new PerformDatabaseBackupNotification(
            message: string.Format(Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Failure, failure),
            type: NotificationType.Error
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileFinish() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_PrepareBackupFile_Finish,
            type: NotificationType.Information
        );
    }
}
