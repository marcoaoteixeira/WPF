using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckSystemUpdateResponse {
    public string? Version { get; }

    public string? DownloadUrl { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: true, nameof(Version), nameof(DownloadUrl))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private CheckSystemUpdateResponse(string? version, string? downloadUrl, string? error) {
        Version = version;
        DownloadUrl = downloadUrl;
        Error = error;
    }

    public static CheckSystemUpdateResponse Success(string version, string downloadUrl) {
        return new CheckSystemUpdateResponse(version, downloadUrl, error: null);
    }

    public static CheckSystemUpdateResponse Failure(string error) {
        return new CheckSystemUpdateResponse(version: null, downloadUrl: null, error);
    }
}