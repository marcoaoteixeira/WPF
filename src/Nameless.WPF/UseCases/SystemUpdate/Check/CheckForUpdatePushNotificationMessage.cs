using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckForUpdatePushNotificationMessage : PushNotificationMessage {
    public CheckForUpdatePushNotificationMessage() {
        Title = Strings.CheckForUpdateNotification_Title;
    }
}