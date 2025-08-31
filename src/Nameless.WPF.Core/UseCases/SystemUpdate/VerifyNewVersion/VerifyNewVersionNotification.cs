using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.VerifyNewVersion;

public record VerifyNewVersionNotification : INotification {
    public string Title => "Verificação de Nova Versão";
    public string Message { get; }
    public NotificationType Type { get; }
    public string? NewVersion { get; }
    public string? DownloadUrl { get; }

    private VerifyNewVersionNotification(
        string message,
        NotificationType type = NotificationType.Information,
        string? newVersion = null,
        string? downloadUrl = null) {
        Message = message;
        Type = type;
        NewVersion = newVersion;
        DownloadUrl = downloadUrl;
    }

    public static VerifyNewVersionNotification Failure(string error) {
        return new VerifyNewVersionNotification(
            message: error,
            type: NotificationType.Error
        );
    }

    public static VerifyNewVersionNotification Success(string newVersion, string downloadUrl) {
        return new VerifyNewVersionNotification(
            message: $"Uma nova versão está disponível. É possível fazer o download da nova versão {newVersion} no endereço: {downloadUrl}",
            type: NotificationType.Success,
            newVersion: newVersion,
            downloadUrl: downloadUrl
        );
    }

    public static VerifyNewVersionNotification Success() {
        return new VerifyNewVersionNotification(
            message: "A versão atual é a mais recente.",
            type: NotificationType.Success
        );
    }

    public static VerifyNewVersionNotification Information(string message) {
        return new VerifyNewVersionNotification(
            message: message,
            type: NotificationType.Information
        );
    }
}