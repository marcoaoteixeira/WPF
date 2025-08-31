using System.IO;
using System.Net.Http;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.DownloadLatestVersion;

public class DownloadLatestVersionRequestHandler : IRequestHandler<DownloadLatestVersionRequest, DownloadLatestVersionResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly HttpClient _httpClient;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;

    public DownloadLatestVersionRequestHandler(IFileSystem fileSystem, HttpClient httpClient, INotificationService notificationService, TimeProvider timeProvider) {
        _fileSystem = Guard.Against.Null(fileSystem);
        _httpClient = Guard.Against.Null(httpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _timeProvider = Guard.Against.Null(timeProvider);
    }

    public async Task<DownloadLatestVersionResponse> HandleAsync(DownloadLatestVersionRequest request, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.DownloadUrl);

        const string UpdateDirectoryPath = "updates";

        try {
            await _notificationService.PublishAsync(DownloadLatestVersionNotification.Starting())
                                      .SuppressContext();

            var response = await _httpClient.GetAsync(request.DownloadUrl, cancellationToken)
                                            .SuppressContext();

            response.EnsureSuccessStatusCode();

            // Ensure "updates" directory exists
            _fileSystem.GetDirectory(UpdateDirectoryPath).Create();

            var fileName = $"{_timeProvider.GetUtcNow():yyyyMMddHHmmss}_v{request.Version}.zip";
            var filePath = Path.Combine(UpdateDirectoryPath, fileName);
            var file = _fileSystem.GetFile(filePath);

            await _notificationService.PublishAsync(DownloadLatestVersionNotification.WriteFile())
                                      .SuppressContext();

            await using var fileStream = file.Open(FileMode.OpenOrCreate, FileAccess.Write);
            await using var httpStream = await response.Content
                                                       .ReadAsStreamAsync(cancellationToken)
                                                       .SuppressContext();

            await httpStream.CopyToAsync(fileStream, cancellationToken)
                            .SuppressContext();

            await _notificationService.PublishAsync(DownloadLatestVersionNotification.Success(file.Path))
                                      .SuppressContext();

            return DownloadLatestVersionResponse.Success();
        }
        catch (Exception ex) {
            await _notificationService.PublishAsync(DownloadLatestVersionNotification.Failure(ex.Message))
                                      .SuppressContext();

            return DownloadLatestVersionResponse.Failure(ex.Message);
        }
    }
}