using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckSystemUpdateNotification : INotification {
    public string Title => Strings.CheckSystemUpdateNotification_Title;
    public string Message { get; }
    public NotificationType Type { get; }
    public string? NewVersion { get; }
    public string? DownloadUrl { get; }

    private CheckSystemUpdateNotification(
        string message,
        NotificationType type = NotificationType.Information,
        string? newVersion = null,
        string? downloadUrl = null) {
        Message = message;
        Type = type;
        NewVersion = newVersion;
        DownloadUrl = downloadUrl;
    }

    public static CheckSystemUpdateNotification Failure(string error) {
        return new CheckSystemUpdateNotification(
            message: error,
            type: NotificationType.Error
        );
    }

    public static CheckSystemUpdateNotification Success(string newVersion, string downloadUrl) {
        return new CheckSystemUpdateNotification(
            message: string.Format(Strings.CheckSystemUpdateNotification_Success_NewVersionAvailable, newVersion, downloadUrl),
            type: NotificationType.Success,
            newVersion: newVersion,
            downloadUrl: downloadUrl
        );
    }

    public static CheckSystemUpdateNotification Success() {
        return new CheckSystemUpdateNotification(
            message: Strings.CheckSystemUpdateNotification_Success_CurrentVersionUpToDate,
            type: NotificationType.Success
        );
    }

    public static CheckSystemUpdateNotification Information(string message) {
        return new CheckSystemUpdateNotification(
            message: message,
            type: NotificationType.Information
        );
    }
}