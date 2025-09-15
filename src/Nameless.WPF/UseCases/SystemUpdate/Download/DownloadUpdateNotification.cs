using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadUpdateNotification : INotification {
    public string Title => Strings.DownloadUpdateNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    private DownloadUpdateNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }

    public static DownloadUpdateNotification Success(string filePath) {
        var message = string.Format(Strings.DownloadUpdateNotification_Success, filePath);

        return new DownloadUpdateNotification(
            message,
            NotificationType.Success
        );
    }

    public static DownloadUpdateNotification Failure(string error) {
        return new DownloadUpdateNotification(
            message: error,
            type: NotificationType.Error
        );
    }

    public static DownloadUpdateNotification Starting() {
        return new DownloadUpdateNotification(
            message: Strings.DownloadUpdateNotification_Starting,
            type: NotificationType.Information
        );
    }

    public static DownloadUpdateNotification WritingFile() {
        return new DownloadUpdateNotification(
            message: Strings.DownloadUpdateNotification_WritingFile,
            type: NotificationType.Information
        );
    }
}
