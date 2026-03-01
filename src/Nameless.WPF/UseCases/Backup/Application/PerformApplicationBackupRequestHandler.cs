using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Nameless.Compression;
using Nameless.Compression.Requests;
using Nameless.IO.FileSystem;
using Nameless.Mediator;
using Nameless.Mediator.Requests;
using Nameless.ObjectModel;
using Nameless.Results;
using Nameless.WPF.Notifications;
using Nameless.WPF.UseCases.Backup.Database.Lucene;
using Nameless.WPF.UseCases.Backup.Database.Sqlite;

namespace Nameless.WPF.UseCases.Backup.Application;

public class PerformApplicationBackupRequestHandler : IRequestHandler<PerformApplicationBackupRequest, PerformApplicationBackupResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly IMediator _mediator;
    private readonly IPushNotification _pushNotification;
    private readonly TimeProvider _timeProvider;
    private readonly IZipFileService _zipFileService;
    private readonly ILogger<PerformApplicationBackupRequestHandler> _logger;

    public PerformApplicationBackupRequestHandler(
        IFileSystem fileSystem,
        IMediator mediator,
        IPushNotification pushNotification,
        TimeProvider timeProvider,
        IZipFileService zipFileService,
        ILogger<PerformApplicationBackupRequestHandler> logger) {
        _fileSystem = fileSystem;
        _mediator = mediator;
        _pushNotification = pushNotification;
        _timeProvider = timeProvider;
        _zipFileService = zipFileService;
        _logger = logger;
    }

    public async Task<PerformApplicationBackupResponse> HandleAsync(PerformApplicationBackupRequest request, CancellationToken cancellationToken) {
        _fileSystem.EnsureBackupsDirectoryExistence();

        await _pushNotification.NotifyStartingAsync()
                               .SkipContextSync();

        var backupDate = _timeProvider.GetUtcNow();

        var sqliteBackup = _mediator.ExecuteAsync(
            new PerformSqliteBackupRequest { BackupDate = backupDate },
            cancellationToken
        );

        var luceneBackup = _mediator.ExecuteAsync(
            new PerformLuceneBackupRequest { BackupDate = backupDate },
            cancellationToken
        );

        await Task.WhenAll(sqliteBackup, luceneBackup)
                  .SkipContextSync();

        var sqliteResponse = await sqliteBackup;
        var luceneResponse = await luceneBackup;

        if (sqliteResponse.Failure || luceneResponse.Failure) {
            await _pushNotification.NotifyFailureAsync()
                                   .SkipContextSync();

            Error[] result = [
                ..GetErrors(sqliteResponse),
                ..GetErrors(luceneResponse)
            ];

            return result;
        }

        // Here we combine the backup files into a single application backup file.
        var sqliteBackupFile = sqliteResponse.Value.BackupFilePath;
        var luceneBackupFile = luceneResponse.Value.BackupFilePath;

        var destinationFilePath = GetDestinationFilePath();
        var compressRequest = new CompressRequest(destinationFilePath) {
            CompressionLevel = CompressionLevel.NoCompression
        }.IncludeFile(sqliteBackupFile)
         .IncludeFile(luceneBackupFile);

        var compressResponse = await _zipFileService.CompressAsync(compressRequest, cancellationToken)
                                                    .SkipContextSync();

        if (compressResponse.Failure) {
            _logger.ExecuteApplicationBackupFailure(compressResponse.Errors[0].Message);

            await _pushNotification.NotifyFailureAsync()
                                   .SkipContextSync();

            return compressResponse.Errors;
        }

        await CleanUpAsync();

        await _pushNotification.NotifySuccessAsync(compressResponse.Value.DestinationFilePath)
                               .SkipContextSync();

        return new PerformApplicationBackupMetadata(
            BackupFilePath: compressResponse.Value.DestinationFilePath
        );

        string GetDestinationFilePath() {
            var fileName = string.Format(
                Constants.BackupDefinitions.FileNamePattern,
                backupDate,
                Constants.BackupDefinitions.ApplicationExtension
            );

            return _fileSystem.GetFullPath(
                Path.Combine(
                    Constants.FolderStructure.BackupsDirectoryName,
                    fileName
                )
            );
        }

        static Error[] GetErrors<T>(Result<T> result) {
            return result.Failure ? result.Errors : [];
        }

        async Task CleanUpAsync() {
            await _pushNotification.NotifyCleanUpAsync()
                                   .SkipContextSync();

            _fileSystem.GetFile(sqliteBackupFile).Delete();
            _fileSystem.GetFile(luceneBackupFile).Delete();
        }
    }
}