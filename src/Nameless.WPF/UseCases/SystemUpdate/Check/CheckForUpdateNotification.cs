using System.Diagnostics.CodeAnalysis;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckForUpdateNotification : INotification {
    public string Title => Strings.CheckForUpdateNotification_Title;

    public string Message { get; }

    public NotificationType Type { get; }

    public string? NewVersion { get; }

    [MemberNotNullWhen(returnValue: true, nameof(NewVersion))]
    public bool NewVersionAvailable => !string.IsNullOrWhiteSpace(NewVersion);

    public CheckForUpdateNotification(string message, NotificationType type, string? newVersion) {
        Message = message;
        Type = type;
        NewVersion = newVersion;
    }
}