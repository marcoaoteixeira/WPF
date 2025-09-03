using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public record DownloadSystemUpdateResponse {
    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private DownloadSystemUpdateResponse(string? error) {
        Error = error;
    }

    public static DownloadSystemUpdateResponse Success() {
        return new DownloadSystemUpdateResponse(error: null);
    }

    public static DownloadSystemUpdateResponse Failure(string error) {
        return new DownloadSystemUpdateResponse(error);
    }
}