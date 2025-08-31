using Microsoft.Extensions.Options;
using Nameless.Infrastructure;
using Nameless.Mediator.Requests;
using Nameless.WPF.GitHub;
using Nameless.WPF.GitHub.Requests;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.VerifyNewVersion;

public class VerifyNewVersionRequestHandler : IRequestHandler<VerifyNewVersionRequest, VerifyNewVersionResponse> {
    private const string NOTIFICATION_VERIFYING_FOR_NEW_VERSION = "Por favor, aguarde enquanto a verificação é realizada...";

    private readonly IApplicationContext _applicationContext;
    private readonly IGitHubHttpClient _gitHubHttpClient;
    private readonly INotificationService _notificationService;
    private readonly IOptions<GitHubOptions> _options;

    public VerifyNewVersionRequestHandler(IApplicationContext applicationContext, IGitHubHttpClient gitHubHttpClient, INotificationService notificationService, IOptions<GitHubOptions> options) {
        _applicationContext = Guard.Against.Null(applicationContext);
        _gitHubHttpClient = Guard.Against.Null(gitHubHttpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _options = Guard.Against.Null(options);
    }

    public async Task<VerifyNewVersionResponse> HandleAsync(VerifyNewVersionRequest request, CancellationToken cancellationToken) {
        await _notificationService.PublishAsync(VerifyNewVersionNotification.Information(NOTIFICATION_VERIFYING_FOR_NEW_VERSION))
                                  .SuppressContext();

        var options = _options.Value;
        var getLatestReleaseRequest = new GetLastestReleaseRequest(options.Owner, options.Repository);
        var getLastestReleaseResponse = await _gitHubHttpClient.GetLastestReleaseAsync(getLatestReleaseRequest, cancellationToken)
                                                               .SuppressContext();

        if (!getLastestReleaseResponse.Succeeded) {
            await _notificationService.PublishAsync(VerifyNewVersionNotification.Failure(getLastestReleaseResponse.Error))
                                      .SuppressContext();

            return VerifyNewVersionResponse.Failure(getLastestReleaseResponse.Error);
        }

        var latestVersion = getLastestReleaseResponse.Release.Name[1..];

        if (_applicationContext.Version.Equals(latestVersion)) {
            await _notificationService.PublishAsync(VerifyNewVersionNotification.Success())
                                      .SuppressContext();

            return VerifyNewVersionResponse.Success(string.Empty, string.Empty);
        }

        var downloadUrl = getLastestReleaseResponse.Release.ZipballUrl;

        var notification = VerifyNewVersionNotification.Success(latestVersion, downloadUrl);
        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        return VerifyNewVersionResponse.Success(latestVersion, downloadUrl);
    }
}