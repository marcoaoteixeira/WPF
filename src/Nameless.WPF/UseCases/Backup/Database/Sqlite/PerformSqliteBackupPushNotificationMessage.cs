using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

public record PerformSqliteBackupPushNotificationMessage : PushNotificationMessage {
    public PerformSqliteBackupPushNotificationMessage() {
        Title = Strings.PerformSqliteBackup_PushNotificationMessage_Title;
    }
}
