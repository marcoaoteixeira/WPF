using System.IO;
using Nameless.Compression;
using Nameless.Compression.Requests;
using Nameless.IO.FileSystem;
using Nameless.Lucene.Infrastructure;
using Nameless.Mediator.Requests;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

public class PerformLuceneBackupRequestHandler : IRequestHandler<PerformLuceneBackupRequest, PerformLuceneBackupResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly IIndexProvider _indexProvider;
    private readonly IPushNotification _pushNotification;
    private readonly IZipFileService _zipFileService;

    public PerformLuceneBackupRequestHandler(
        IFileSystem fileSystem,
        IIndexProvider indexProvider,
        IPushNotification pushNotification,
        IZipFileService zipFileService) {
        _fileSystem = fileSystem;
        _indexProvider = indexProvider;
        _pushNotification = pushNotification;
        _zipFileService = zipFileService;
    }

    public async Task<PerformLuceneBackupResponse> HandleAsync(PerformLuceneBackupRequest request, CancellationToken cancellationToken) {
        _fileSystem.EnsureTemporaryDirectoryExistence();

        await _pushNotification.NotifyStartingAsync()
                               .SkipContextSync();

        // First, dispose the current index to ensure all data is flushed to disk.
        // This is crucial before performing a backup. Also, it will remove the
        // index from the index provider's cache.
        _indexProvider.Get(Constants.Lucene.UniqueIndexName).Dispose();

        // Next, perform the backup operation.
        var sourceDirectoryPath = GetSourceDirectory();
        var destinationFilePath = GetDestinationFilePath(request.BackupDate);

        var compressRequest = new CompressRequest(destinationFilePath).IncludeDirectory(sourceDirectoryPath);
        var compressResponse = await _zipFileService.CompressAsync(compressRequest, cancellationToken)
                                                    .SkipContextSync();

        if (compressResponse.Failure) {
            await _pushNotification.NotifyFailureAsync(compressResponse.Errors[0].Message)
                                   .SkipContextSync();

            return compressResponse.Errors[0];
        }

        await _pushNotification.NotifySuccessAsync(compressResponse.Value.DestinationFilePath)
                               .SkipContextSync();

        return new PerformLuceneBackupMetadata(
            BackupFilePath: compressResponse.Value.DestinationFilePath
        );
    }

    private string GetSourceDirectory() {
        return _fileSystem.GetFullPath(
            Path.Combine(
                Constants.FolderStructure.DatabasesDirectoryName,
                Constants.Lucene.UniqueIndexName
            )
        );
    }

    private string GetDestinationFilePath(DateTimeOffset backupDate) {
        var fileName = string.Format(
            Constants.BackupDefinitions.FileNamePattern,
            backupDate,
            Constants.BackupDefinitions.LuceneExtension
        );

        return _fileSystem.GetFullPath(
            Path.Combine(
                Constants.FolderStructure.TemporaryDirectoryName,
                fileName
            )
        );
    }
}