using System.Net.Http;
using System.Net.Http.Json;
using Nameless.WPF.GitHub.ObjectModel;
using Nameless.WPF.GitHub.Requests;
using Nameless.WPF.GitHub.Responses;

namespace Nameless.WPF.GitHub;

public class GitHubHttpClient : IGitHubHttpClient {
    private readonly HttpClient _httpClient;

    /// <summary>
    ///     Initializes a new instance
    ///     of <see cref="GitHubHttpClient"/> class.
    /// </summary>
    /// <param name="httpClient">
    ///     The underlying HTTP client.
    /// </param>
    public GitHubHttpClient(HttpClient httpClient) {
        _httpClient = Guard.Against.Null(httpClient);
    }

    /// <inheritdoc />
    public async Task<GetLastestReleaseResponse> GetLastestReleaseAsync(GetLastestReleaseRequest request, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.Owner);
        Guard.Against.NullOrWhiteSpace(request.Repository);

        var statusCode = 200;

        try {
            var url = $"/repos/{request.Owner}/{request.Repository}/releases/latest";

            var response = await _httpClient.GetAsync(url, cancellationToken)
                                            .SuppressContext();

            statusCode = (int)response.StatusCode;

            response.EnsureSuccessStatusCode();

            var release = await response.Content
                                        .ReadFromJsonAsync<Release>(cancellationToken)
                                        .SuppressContext();

            return release is not null
                ? GetLastestReleaseResponse.Success(release)
                : GetLastestReleaseResponse.DeserializationFailure(statusCode);
        }
        catch (Exception ex) {
            return GetLastestReleaseResponse.Failure(statusCode, ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<GetReleaseAssetsResponse> GetReleaseAssetsAsync(GetReleaseAssetsRequest request, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.Owner);
        Guard.Against.NullOrWhiteSpace(request.Repository);
        Guard.Against.LowerThan(request.ReleaseID, compare: 0);

        var statusCode = 200;

        try {
            var url = $"/repos/{request.Owner}/{request.Repository}/releases/{request.ReleaseID}/assets";

            var response = await _httpClient.GetAsync(url, cancellationToken)
                                            .SuppressContext();

            statusCode = (int)response.StatusCode;

            response.EnsureSuccessStatusCode();

            var release = await response.Content
                                        .ReadFromJsonAsync<ReleaseAsset[]>(cancellationToken)
                                        .SuppressContext();

            return release is not null
                ? GetReleaseAssetsResponse.Success(release)
                : GetReleaseAssetsResponse.DeserializationFailure(statusCode);
        }
        catch (Exception ex) {
            return GetReleaseAssetsResponse.Failure(statusCode, ex.Message);
        }
    }
}
