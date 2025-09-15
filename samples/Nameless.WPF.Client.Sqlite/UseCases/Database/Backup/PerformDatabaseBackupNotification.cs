using Nameless.WPF.Client.Sqlite.Resources;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

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

    public static PerformDatabaseBackupNotification BackupDatabaseDataStarting() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_BackupDatabaseDataStarting,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification BackupDatabaseDataFailure(string failure) {
        return new PerformDatabaseBackupNotification(
            message: string.Format(Strings.PerformDatabaseBackupNotification_BackupDatabaseDataFailure, failure),
            type: NotificationType.Error
        );
    }

    public static PerformDatabaseBackupNotification BackupDatabaseDataFinish() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_BackupDatabaseDataFinish,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileStarting() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_PrepareBackupFileStarting,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileFailure(string failure) {
        return new PerformDatabaseBackupNotification(
            message: string.Format(Strings.PerformDatabaseBackupNotification_PrepareBackupFileFailure, failure),
            type: NotificationType.Error
        );
    }

    public static PerformDatabaseBackupNotification PrepareBackupFileFinish() {
        return new PerformDatabaseBackupNotification(
            message: Strings.PerformDatabaseBackupNotification_PrepareBackupFileFinish,
            type: NotificationType.Information
        );
    }

    public static PerformDatabaseBackupNotification Info() {
        return new PerformDatabaseBackupNotification(
            message: "Ok",
            type: NotificationType.Information
        );
    }
}
