namespace Nameless.WPF.UseCases.SystemUpdate.DownloadLatestVersion;

public record DownloadLatestVersionResponse : Response {
    private DownloadLatestVersionResponse(string? error)
        : base(error) { }

    public static DownloadLatestVersionResponse Success() {
        return new DownloadLatestVersionResponse(error: null);
    }

    public static DownloadLatestVersionResponse Failure(string error) {
        return new DownloadLatestVersionResponse(error);
    }
}