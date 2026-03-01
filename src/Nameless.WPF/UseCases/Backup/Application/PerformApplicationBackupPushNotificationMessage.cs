using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.Backup.Application;

public record PerformApplicationBackupPushNotificationMessage : PushNotificationMessage {
    public PerformApplicationBackupPushNotificationMessage() {
        Title = Strings.PerformApplicationBackup_PushNotificationMessage_Title;
    }
}