using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public record DownloadUpdateResponse {
    public string? FilePath { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: true, nameof(FilePath))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private DownloadUpdateResponse(string? filePath, string? error) {
        FilePath = filePath;
        Error = error;
    }

    public static DownloadUpdateResponse Success(string filePath) {
        return new DownloadUpdateResponse(filePath, error: null);
    }

    public static DownloadUpdateResponse Failure(string error) {
        return new DownloadUpdateResponse(filePath: null, error);
    }
}