using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

public record FetchNewVersionInformationResponse {
    public string? Url { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private FetchNewVersionInformationResponse(string? url, string? error) {
        Url = url;
        Error = error;
    }

    public static FetchNewVersionInformationResponse Success(string url) {
        return new FetchNewVersionInformationResponse(
            url,
            error: null
        );
    }

    public static FetchNewVersionInformationResponse Skip() {
        return new FetchNewVersionInformationResponse(
            url: null,
            error: null
        );
    }

    public static FetchNewVersionInformationResponse Failure(string error) {
        return new FetchNewVersionInformationResponse(
            url: null,
            error: error
        );
    }
}