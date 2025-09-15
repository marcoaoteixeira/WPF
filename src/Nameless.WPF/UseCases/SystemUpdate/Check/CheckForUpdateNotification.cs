using System.Diagnostics.CodeAnalysis;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckForUpdateNotification : INotification {
    public string Title => Strings.CheckForUpdateNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    public string? NewVersion { get; }

    public string? DownloadUrl { get; }

    [MemberNotNullWhen(returnValue: true, nameof(NewVersion), nameof(DownloadUrl))]
    public bool NewVersionAvailable => !string.IsNullOrWhiteSpace(DownloadUrl);

    private CheckForUpdateNotification(
        string message,
        NotificationType type = NotificationType.Information,
        string? newVersion = null,
        string? downloadUrl = null) {
        Message = message;
        Type = type;
        NewVersion = newVersion;
        DownloadUrl = downloadUrl;
    }

    public static CheckForUpdateNotification Failure(string error) {
        return new CheckForUpdateNotification(
            message: error,
            type: NotificationType.Error
        );
    }

    public static CheckForUpdateNotification Success(string newVersion, string downloadUrl) {
        return new CheckForUpdateNotification(
            message: string.Format(Strings.CheckForUpdateNotification_Success_NewVersionAvailable, newVersion, downloadUrl),
            type: NotificationType.Success,
            newVersion: newVersion,
            downloadUrl: downloadUrl
        );
    }

    public static CheckForUpdateNotification Success() {
        return new CheckForUpdateNotification(
            message: Strings.CheckForUpdateNotification_Success_CurrentVersionUpToDate,
            type: NotificationType.Success
        );
    }

    public static CheckForUpdateNotification Starting() {
        return new CheckForUpdateNotification(
            message: Strings.CheckForUpdateRequestHandler_Starting,
            type: NotificationType.Information
        );
    }
}