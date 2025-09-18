using Microsoft.Extensions.Options;
using Nameless.Infrastructure;
using Nameless.Mediator.Requests;
using Nameless.WPF.GitHub;
using Nameless.WPF.GitHub.Requests;
using Nameless.WPF.Helpers;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public class CheckForUpdateRequestHandler : IRequestHandler<CheckForUpdateRequest, CheckForUpdateResponse> {
    private readonly IApplicationContext _applicationContext;
    private readonly IGitHubHttpClient _gitHubHttpClient;
    private readonly INotificationService _notificationService;
    private readonly IOptions<GitHubOptions> _options;

    public CheckForUpdateRequestHandler(IApplicationContext applicationContext, IGitHubHttpClient gitHubHttpClient, INotificationService notificationService, IOptions<GitHubOptions> options) {
        _applicationContext = Guard.Against.Null(applicationContext);
        _gitHubHttpClient = Guard.Against.Null(gitHubHttpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _options = Guard.Against.Null(options);
    }

    public async Task<CheckForUpdateResponse> HandleAsync(CheckForUpdateRequest request, CancellationToken cancellationToken) {
        var notification = CheckForUpdateNotification.Starting();
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        var options = _options.Value;
        var getLatestReleaseRequest = new GetLastestReleaseRequest(options.Owner, options.Repository);
        var getLastestReleaseResponse = await _gitHubHttpClient.GetLastestReleaseAsync(getLatestReleaseRequest, cancellationToken)
                                                               .SuppressContext();

        if (!getLastestReleaseResponse.Succeeded) {
            notification = CheckForUpdateNotification.Failure(getLastestReleaseResponse.Error);
            await _notificationService.PublishAsync(notification)
                                      .SuppressContext();

            return CheckForUpdateResponse.Failure(getLastestReleaseResponse.Error);
        }

        var currentVersion = VersionHelper.Parse(_applicationContext.Version);
        var latestVersion = VersionHelper.Parse(getLastestReleaseResponse.Release.TagName);

        if (currentVersion >= latestVersion) {
            await _notificationService.PublishAsync(CheckForUpdateNotification.Success())
                                      .SuppressContext();

            return CheckForUpdateResponse.Skip();
        }

        notification = CheckForUpdateNotification.Success(latestVersion.ToString(3));
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        return CheckForUpdateResponse.Success(
            releaseID: getLastestReleaseResponse.Release.Id,
            applicationName: _applicationContext.ApplicationName,
            version: latestVersion.ToString(3)
        );
    }
}