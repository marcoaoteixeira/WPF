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
        _applicationContext = applicationContext;
        _gitHubHttpClient = gitHubHttpClient;
        _notificationService = notificationService;
        _options = options;
    }

    public async Task<CheckForUpdateResponse> HandleAsync(CheckForUpdateRequest request, CancellationToken cancellationToken) {
        await _notificationService.NotifyStartingAsync()
                                  .SkipContextSync();

        var options = _options.Value;
        var getLatestReleaseRequest = new GetLastestReleaseRequest(options.Owner, options.Repository);
        var getLastestReleaseResponse = await _gitHubHttpClient.GetLastestReleaseAsync(getLatestReleaseRequest, cancellationToken)
                                                               .SkipContextSync();

        if (!getLastestReleaseResponse.Success) {
            await _notificationService.NotifyFailureAsync(getLastestReleaseResponse.Errors[0].Message)
                                      .SkipContextSync();

            return getLastestReleaseResponse.Errors[0];
        }

        var currentVersion = VersionHelper.Parse(_applicationContext.Version);
        var latestVersion = VersionHelper.Parse(getLastestReleaseResponse.Value.TagName);

        if (currentVersion >= latestVersion) {
            await _notificationService.NotifySuccessAsync()
                                      .SkipContextSync();

            return (CheckForUpdateMetadata)default;
        }

        await _notificationService.NotifySuccessAsync(latestVersion.ToString(3))
                                  .SkipContextSync();

        return new CheckForUpdateMetadata(
            ReleaseID: getLastestReleaseResponse.Value.Id,
            ApplicationName: _applicationContext.ApplicationName,
            Version: latestVersion.ToString(3)
        );
    }
}