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

    public static FetchNewVersionInformationNotification Starting() {
        return new FetchNewVersionInformationNotification(
            message: Strings.FetchNewVersionInformationNotification_Starting,
            type: NotificationType.Information
        );
    }

    public static FetchNewVersionInformationNotification Success() {
        return new FetchNewVersionInformationNotification(
            message: Strings.FetchNewVersionInformationNotification_Success,
            type: NotificationType.Success
        );
    }

    public static FetchNewVersionInformationNotification NotFound() {
        return new FetchNewVersionInformationNotification(
            message: Strings.FetchNewVersionInformationNotification_NotFound,
            type: NotificationType.Error
        );
    }

    public static FetchNewVersionInformationNotification Failure(string version, string error) {
        return new FetchNewVersionInformationNotification(
            message: string.Format(Strings.FetchNewVersionInformationNotification_Failure, version, error),
            type: NotificationType.Error
        );
    }
}