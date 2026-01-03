using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadUpdateNotification : INotification {
    public string Title => Strings.DownloadUpdateNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    public DownloadUpdateNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }
}
