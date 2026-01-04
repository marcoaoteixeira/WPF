using Nameless.WPF.Client.Sqlite.Resources;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public record PerformDatabaseBackupNotification : INotification {
    public string Title => Strings.PerformDatabaseBackupNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    public PerformDatabaseBackupNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }
}
