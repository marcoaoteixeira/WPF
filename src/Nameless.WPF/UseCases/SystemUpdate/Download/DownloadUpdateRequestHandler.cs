using System.IO;
using System.Net.Http;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadUpdateRequestHandler : IRequestHandler<DownloadUpdateRequest, DownloadUpdateResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly HttpClient _httpClient;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;

    public DownloadUpdateRequestHandler(IFileSystem fileSystem, HttpClient httpClient, INotificationService notificationService, TimeProvider timeProvider) {
        _fileSystem = Guard.Against.Null(fileSystem);
        _httpClient = Guard.Against.Null(httpClient);
        _notificationService = Guard.Against.Null(notificationService);
        _timeProvider = Guard.Against.Null(timeProvider);
    }

    public async Task<DownloadUpdateResponse> HandleAsync(DownloadUpdateRequest request, CancellationToken cancellationToken) {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.DownloadUrl);

        try {
            await _notificationService.PublishAsync(DownloadUpdateNotification.Starting())
                                      .SuppressContext();

            var response = await _httpClient.GetAsync(request.DownloadUrl, cancellationToken)
                                            .SuppressContext();

            response.EnsureSuccessStatusCode();

            // Ensure "updates" directory exists
            _fileSystem.GetDirectory(Constants.SystemUpdate.DIRECTORY_NAME).Create();

            var fileName = $"{_timeProvider.GetUtcNow():yyyyMMddHHmmss}_v{request.Version}.zip";
            var filePath = Path.Combine(Constants.SystemUpdate.DIRECTORY_NAME, fileName);
            var file = _fileSystem.GetFile(filePath);

            await _notificationService.PublishAsync(DownloadUpdateNotification.WritingFile())
                                      .SuppressContext();

            await using var fileStream = file.Open();
            await using var httpStream = await response.Content
                                                       .ReadAsStreamAsync(cancellationToken)
                                                       .SuppressContext();

            await httpStream.CopyToAsync(fileStream, cancellationToken)
                            .SuppressContext();

            httpStream.Close();
            fileStream.Close();

            await _notificationService.PublishAsync(DownloadUpdateNotification.Success(file.Path))
                                      .SuppressContext();

            return DownloadUpdateResponse.Success(file.Path);
        }
        catch (Exception ex) {
            await _notificationService.PublishAsync(DownloadUpdateNotification.Failure(ex.Message))
                                      .SuppressContext();

            return DownloadUpdateResponse.Failure(ex.Message);
        }
    }
}