using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.DownloadLatestVersion;

public class DownloadLatestVersionNotification : INotification {
    public string Title => "Download de Nova Versão";
    public string Message { get; }
    public NotificationType Type { get; }

    private DownloadLatestVersionNotification(string message, NotificationType type) {
        Message = message;
        Type = type;
    }

    public static DownloadLatestVersionNotification Success(string filePath) {
        var message = $"Download da nova versão concluído com sucesso. O arquivo da nova versão foi salvo em: {filePath}";

        return new DownloadLatestVersionNotification(message, NotificationType.Success);
    }

    public static DownloadLatestVersionNotification Failure(string error) {
        return new DownloadLatestVersionNotification(message: error, type: NotificationType.Error);
    }

    public static DownloadLatestVersionNotification Information(string message) {
        return new DownloadLatestVersionNotification(message: message, type: NotificationType.Information);
    }

    public static DownloadLatestVersionNotification Starting() {
        return Information("Iniciando o download da nova versão...");
    }

    public static DownloadLatestVersionNotification WriteFile() {
        return Information("Gravando arquivo em disco...");
    }
}
