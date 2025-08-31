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
        Guard.Against.NullOrWhiteSpace(request.Owner);
        Guard.Against.NullOrWhiteSpace(request.Repository);

        try {
            var url = $"/repos/{request.Owner}/{request.Repository}/releases/latest";

            var response = await _httpClient.GetAsync(url, cancellationToken)
                                            .SuppressContext();

            response.EnsureSuccessStatusCode();

            var release = await response.Content
                                        .ReadFromJsonAsync<Release>(cancellationToken)
                                        .SuppressContext();

            return release is not null
                ? GetLastestReleaseResponse.Success(release)
                : GetLastestReleaseResponse.Failure("Couldn't retrieve latest release information.");
        }
        catch (Exception ex) {
            return GetLastestReleaseResponse.Failure($"An error occurred while trying to retrieve latest release information: {ex.Message}");
        }
    }
}
