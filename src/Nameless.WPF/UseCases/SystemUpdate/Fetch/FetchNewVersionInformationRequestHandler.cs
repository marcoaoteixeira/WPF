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
        _httpClient = Guard.Against.Null(httpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _options = Guard.Against.Null(options);
    }

    public async Task<FetchNewVersionInformationResponse> HandleAsync(FetchNewVersionInformationRequest request, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.LowerThan(request.ReleaseID, compare: 0);
        Guard.Against.NullOrWhiteSpace(request.ApplicationName);
        Guard.Against.NullOrWhiteSpace(request.Version);

        var notification = FetchNewVersionInformationNotification.Starting();
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        var options = _options.Value;
        var getReleaseAssetsRequest = new GetReleaseAssetsRequest(
            options.Owner,
            options.Repository,
            request.ReleaseID
        );
        var getReleaseAssetsResponse = await _httpClient.GetReleaseAssetsAsync(getReleaseAssetsRequest, cancellationToken)
                                                        .SuppressContext();

        if (!getReleaseAssetsResponse.Succeeded) {
            notification = FetchNewVersionInformationNotification.Failure(getReleaseAssetsResponse.Error, request.Version);
            await _notificationService.PublishAsync(notification)
                                      .SuppressContext();

            return FetchNewVersionInformationResponse.Failure(getReleaseAssetsResponse.Error);
        }

        var assetName = $"{request.ApplicationName}.v{request.Version}.zip";
        var asset = getReleaseAssetsResponse.Assets
                                            .SingleOrDefault(item => item.Name == assetName);

        if (asset is null) {
            notification = FetchNewVersionInformationNotification.NotFound();
            await _notificationService.PublishAsync(notification)
                                      .SuppressContext();

            return FetchNewVersionInformationResponse.Skip();
        }

        notification = FetchNewVersionInformationNotification.Success();
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        return FetchNewVersionInformationResponse.Success(asset.BrowserDownloadUrl);
    }
}