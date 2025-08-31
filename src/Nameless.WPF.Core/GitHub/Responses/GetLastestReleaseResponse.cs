using System.Diagnostics.CodeAnalysis;
using Nameless.WPF.GitHub.ObjectModel;

namespace Nameless.WPF.GitHub.Responses;

public record GetLastestReleaseResponse {
    public Release? Release { get; }
    public string? Error { get; }

    [MemberNotNullWhen(returnValue: true, nameof(Release))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private GetLastestReleaseResponse(Release? release, string? error) {
        Release = release;
        Error = error;
    }

    public static GetLastestReleaseResponse Success(Release release) {
        return new GetLastestReleaseResponse(release, error: null);
    }

    public static GetLastestReleaseResponse Failure(string error) {
        return new GetLastestReleaseResponse(release: null, error);
    }
};