namespace Nameless.WPF.UseCases.SystemUpdate.VerifyNewVersion;

public record VerifyNewVersionResponse : Response {
    public string? Version { get; }
    public string? DownloadUrl { get; }

    private VerifyNewVersionResponse(string? version, string? downloadUrl, string? error)
        : base(error) {
        Version = version;
        DownloadUrl = downloadUrl;
    }

    public static VerifyNewVersionResponse Success(string version, string downloadUrl) {
        return new VerifyNewVersionResponse(version, downloadUrl, error: null);
    }

    public static VerifyNewVersionResponse Failure(string error) {
        return new VerifyNewVersionResponse(version: null, downloadUrl: null, error);
    }
}