using Microsoft.Extensions.Options;
using Nameless.Mediator.Requests;
using Nameless.WPF.GitHub;
using Nameless.WPF.GitHub.Requests;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.Fetch;

public class FetchNewVersionInformationRequestHandler : IRequestHandler<FetchNewVersionInformationRequest, FetchNewVersionInformationResponse> {
    private readonly IGitHubHttpClient _httpClient;
    private readonly INotificationService _notificationService;
    private readonly IOptions<GitHubOptions> _options;

    public FetchNewVersionInformationRequestHandler(IGitHubHttpClient httpClient, INotificationService notificationService, IOptions<GitHubOptions> options) {
        _httpClient = httpClient;
        _notificationService = notificationService;
        _options = options;
    }

    public async Task<FetchNewVersionInformationResponse> HandleAsync(FetchNewVersionInformationRequest request, CancellationToken cancellationToken) {
        await _notificationService.NotifyStartingAsync()
                                  .SkipContextSync();

        var options = _options.Value;
        var getReleaseAssetsRequest = new GetReleaseAssetsRequest(
            options.Owner,
            options.Repository,
            request.ReleaseID
        );
        var getReleaseAssetsResponse = await _httpClient.GetReleaseAssetsAsync(getReleaseAssetsRequest, cancellationToken)
                                                        .SkipContextSync();

        if (!getReleaseAssetsResponse.Success) {
            await _notificationService.NotifyFailureAsync(request.Version, getReleaseAssetsResponse.Errors[0].Message)
                                      .SkipContextSync();

            return getReleaseAssetsResponse.Errors[0];
        }

        var assetName = $"{request.ApplicationName}.v{request.Version}.zip";
        var asset = getReleaseAssetsResponse.Value.SingleOrDefault(item => item.Name == assetName);

        if (asset is null) {
            await _notificationService.NotifyNotFoundAsync()
                                      .SkipContextSync();

            return (FetchNewVersionMetadata)default;
        }

        await _notificationService.NotifySuccessAsync()
                                  .SkipContextSync();

        return new FetchNewVersionMetadata(asset.BrowserDownloadUrl);
    }
}