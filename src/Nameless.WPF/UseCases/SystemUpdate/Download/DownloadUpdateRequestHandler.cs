using System.IO;
using System.Net.Http;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.ObjectModel;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadUpdateRequestHandler : IRequestHandler<DownloadUpdateRequest, DownloadUpdateResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly HttpClient _httpClient;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;

    public DownloadUpdateRequestHandler(IFileSystem fileSystem, HttpClient httpClient, INotificationService notificationService, TimeProvider timeProvider) {
        _fileSystem = fileSystem;
        _httpClient = httpClient;
        _notificationService = notificationService;
        _timeProvider = timeProvider;
    }

    public async Task<DownloadUpdateResponse> HandleAsync(DownloadUpdateRequest request, CancellationToken cancellationToken) {
        try {
            await _notificationService.NotifyStartingAsync()
                                      .SkipContextSync();

            var response = await _httpClient.GetAsync(request.Url, cancellationToken)
                                            .SkipContextSync();

            response.EnsureSuccessStatusCode();

            // Ensure "updates" directory exists
            _fileSystem.GetDirectory(Constants.SystemUpdate.DIRECTORY_NAME).Create();

            var fileName = $"{_timeProvider.GetUtcNow():yyyyMMddHHmmss}_v{request.Version}.zip";
            var filePath = Path.Combine(Constants.SystemUpdate.DIRECTORY_NAME, fileName);
            var file = _fileSystem.GetFile(filePath);

            await _notificationService.NotifyWritingFileAsync()
                                      .SkipContextSync();

            await using var fileStream = file.Open();
            await using var httpStream = await response.Content
                                                       .ReadAsStreamAsync(cancellationToken)
                                                       .SkipContextSync();

            await httpStream.CopyToAsync(fileStream, cancellationToken)
                            .SkipContextSync();

            httpStream.Close();
            fileStream.Close();

            await _notificationService.NotifySuccessAsync(file.Path)
                                      .SkipContextSync();

            return new DownloadUpdateMetadata(file.Path);
        }
        catch (Exception ex) {
            await _notificationService.NotifyFailureAsync(ex.Message)
                                      .SkipContextSync();

            return Error.Failure(ex.Message);
        }
    }
}