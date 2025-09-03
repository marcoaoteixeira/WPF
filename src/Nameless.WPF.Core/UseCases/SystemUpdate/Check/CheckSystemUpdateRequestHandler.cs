using Microsoft.Extensions.Options;
using Nameless.Infrastructure;
using Nameless.Mediator.Requests;
using Nameless.WPF.GitHub;
using Nameless.WPF.GitHub.Requests;
using Nameless.WPF.Notifications;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public class CheckSystemUpdateRequestHandler : IRequestHandler<CheckSystemUpdateRequest, CheckSystemUpdateResponse> {
    private readonly IApplicationContext _applicationContext;
    private readonly IGitHubHttpClient _gitHubHttpClient;
    private readonly INotificationService _notificationService;
    private readonly IOptions<GitHubOptions> _options;

    public CheckSystemUpdateRequestHandler(IApplicationContext applicationContext, IGitHubHttpClient gitHubHttpClient, INotificationService notificationService, IOptions<GitHubOptions> options) {
        _applicationContext = Guard.Against.Null(applicationContext);
        _gitHubHttpClient = Guard.Against.Null(gitHubHttpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _options = Guard.Against.Null(options);
    }

    public async Task<CheckSystemUpdateResponse> HandleAsync(CheckSystemUpdateRequest request, CancellationToken cancellationToken) {
        await _notificationService.PublishAsync(CheckSystemUpdateNotification.Information(Strings.CheckSystemUpdateRequestHandler_NotificationCheckingNewVersion))
                                  .SuppressContext();

        var options = _options.Value;
        var getLatestReleaseRequest = new GetLastestReleaseRequest(options.Owner, options.Repository);
        var getLastestReleaseResponse = await _gitHubHttpClient.GetLastestReleaseAsync(getLatestReleaseRequest, cancellationToken)
                                                               .SuppressContext();

        if (!getLastestReleaseResponse.Succeeded) {
            await _notificationService.PublishAsync(CheckSystemUpdateNotification.Failure(getLastestReleaseResponse.Error))
                                      .SuppressContext();

            return CheckSystemUpdateResponse.Failure(getLastestReleaseResponse.Error);
        }

        var latestVersion = getLastestReleaseResponse.Release.Name[1..];

        if (_applicationContext.Version.Equals(latestVersion)) {
            await _notificationService.PublishAsync(CheckSystemUpdateNotification.Success())
                                      .SuppressContext();

            return CheckSystemUpdateResponse.Success(string.Empty, string.Empty);
        }

        var downloadUrl = getLastestReleaseResponse.Release.ZipballUrl;

        var notification = CheckSystemUpdateNotification.Success(latestVersion, downloadUrl);
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        return CheckSystemUpdateResponse.Success(latestVersion, downloadUrl);
    }
}