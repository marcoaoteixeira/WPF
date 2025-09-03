using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadSystemUpdateNotification : INotification {
    public string Title => Strings.DownloadSystemUpdateNotification_Title;
    public string Message { get; }
    public NotificationType Type { get; }

    private DownloadSystemUpdateNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }

    public static DownloadSystemUpdateNotification Success(string filePath) {
        var message = string.Format(Strings.DownloadSystemUpdateNotification_Success, filePath);

        return new DownloadSystemUpdateNotification(message, NotificationType.Success);
    }

    public static DownloadSystemUpdateNotification Failure(string error) {
        return new DownloadSystemUpdateNotification(message: error, type: NotificationType.Error);
    }

    public static DownloadSystemUpdateNotification Starting() {
        return new DownloadSystemUpdateNotification(message: Strings.DownloadSystemUpdateNotification_Starting, type: NotificationType.Information);
    }

    public static DownloadSystemUpdateNotification WritingFile() {
        return new DownloadSystemUpdateNotification(message: Strings.DownloadSystemUpdateNotification_WritingFile, type: NotificationType.Information);
    }
}
