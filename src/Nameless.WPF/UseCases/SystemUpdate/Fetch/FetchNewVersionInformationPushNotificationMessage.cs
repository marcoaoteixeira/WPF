using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

public record FetchNewVersionInformationPushNotificationMessage : PushNotificationMessage {
    public FetchNewVersionInformationPushNotificationMessage() {
        Title = Strings.FetchNewVersionInformationNotification_Title;
    }
}