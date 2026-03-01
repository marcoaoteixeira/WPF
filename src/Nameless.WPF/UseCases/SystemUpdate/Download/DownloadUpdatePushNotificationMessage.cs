using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public record DownloadUpdatePushNotificationMessage : PushNotificationMessage {
    public DownloadUpdatePushNotificationMessage() {
        Title = Strings.DownloadUpdateNotification_Title;
    }
}
