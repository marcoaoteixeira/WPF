using System.Diagnostics.CodeAnalysis;
using Nameless.WPF.GitHub.ObjectModel;

namespace Nameless.WPF.GitHub.Responses;

public record GetLastestReleaseResponse {
    public Release? Release { get; }

    public int StatusCode { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: true, nameof(Release))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private GetLastestReleaseResponse(Release? release, int statusCode, string? error) {
        Release = release;
        StatusCode = statusCode;
        Error = error;
    }

    public static GetLastestReleaseResponse Success(Release release) {
        return new GetLastestReleaseResponse(release, statusCode: 200, error: null);
    }

    public static GetLastestReleaseResponse Failure(int statusCode, string error) {
        return new GetLastestReleaseResponse(release: null, statusCode, error);
    }
};