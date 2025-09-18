using System.Diagnostics.CodeAnalysis;
using Nameless.WPF.GitHub.ObjectModel;

namespace Nameless.WPF.GitHub.Responses;

public record GetReleaseAssetsResponse {
    public ReleaseAsset[] Assets { get; } = [];

    public int StatusCode { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private GetReleaseAssetsResponse(ReleaseAsset[] assets, int statusCode, string? error) {
        Assets = assets;
        StatusCode = statusCode;
        Error = error;
    }

    public static GetReleaseAssetsResponse Success(ReleaseAsset[] assets) {
        return new GetReleaseAssetsResponse(assets, statusCode: 200, error: null);
    }

    public static GetReleaseAssetsResponse Failure(int statusCode, string error) {
        return new GetReleaseAssetsResponse(assets: [], statusCode, error);
    }
};