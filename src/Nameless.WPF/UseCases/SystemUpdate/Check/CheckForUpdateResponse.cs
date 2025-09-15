using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckForUpdateResponse {
    public string? Version { get; }

    public string? DownloadUrl { get; }

    [MemberNotNullWhen(returnValue: true, nameof(Version), nameof(DownloadUrl))]
    public bool NewVersionAvailable => !string.IsNullOrWhiteSpace(Version) &&
                                       !string.IsNullOrWhiteSpace(DownloadUrl);

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private CheckForUpdateResponse(string? version, string? downloadUrl, string? error) {
        Version = version;
        DownloadUrl = downloadUrl;
        Error = error;
    }

    public static CheckForUpdateResponse Success(string version, string downloadUrl) {
        return new CheckForUpdateResponse(version, downloadUrl, error: null);
    }

    public static CheckForUpdateResponse Failure(string error) {
        return new CheckForUpdateResponse(version: null, downloadUrl: null, error);
    }
}