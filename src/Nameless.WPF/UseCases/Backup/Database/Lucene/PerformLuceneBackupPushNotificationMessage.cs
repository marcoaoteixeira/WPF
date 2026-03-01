using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

public record PerformLuceneBackupPushNotificationMessage : PushNotificationMessage {
    public PerformLuceneBackupPushNotificationMessage() {
        Title = Strings.PerformLuceneBackup_PushNotificationMessage_Title;
    }
}
