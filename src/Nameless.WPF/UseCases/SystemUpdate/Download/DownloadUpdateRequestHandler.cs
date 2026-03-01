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
    private readonly IPushNotification _pushNotification;
    private readonly TimeProvider _timeProvider;

    public DownloadUpdateRequestHandler(IFileSystem fileSystem, HttpClient httpClient, IPushNotification pushNotification, TimeProvider timeProvider) {
        _fileSystem = fileSystem;
        _httpClient = httpClient;
        _pushNotification = pushNotification;
        _timeProvider = timeProvider;
    }

    public async Task<DownloadUpdateResponse> HandleAsync(DownloadUpdateRequest request, CancellationToken cancellationToken) {
        try {
            await _pushNotification.NotifyStartingAsync()
                                   .SkipContextSync();

            var response = await _httpClient.GetAsync(request.Url, cancellationToken)
                                            .SkipContextSync();

            response.EnsureSuccessStatusCode();

            // Ensure "updates" directory exists
            _fileSystem.GetDirectory(Constants.FolderStructure.UpdatesDirectoryName).Create();

            var fileName = $"{_timeProvider.GetUtcNow():yyyyMMddHHmmss}_v{request.Version}.zip";
            var filePath = Path.Combine(Constants.FolderStructure.UpdatesDirectoryName, fileName);
            var file = _fileSystem.GetFile(filePath);

            await _pushNotification.NotifyWritingFileAsync()
                                   .SkipContextSync();

            await using var fileStream = file.Open();
            await using var httpStream = await response.Content
                                                       .ReadAsStreamAsync(cancellationToken)
                                                       .SkipContextSync();

            await httpStream.CopyToAsync(fileStream, cancellationToken)
                            .SkipContextSync();

            httpStream.Close();
            fileStream.Close();

            await _pushNotification.NotifySuccessAsync(file.Path)
                                   .SkipContextSync();

            return new DownloadUpdateMetadata(file.Path);
        }
        catch (Exception ex) {
            await _pushNotification.NotifyFailureAsync(ex.Message)
                                   .SkipContextSync();

            return Error.Failure(ex.Message);
        }
    }
}