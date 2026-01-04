using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

public record FetchNewVersionInformationNotification : INotification {
    public string Title => Strings.FetchNewVersionInformationNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    private FetchNewVersionInformationNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }
}