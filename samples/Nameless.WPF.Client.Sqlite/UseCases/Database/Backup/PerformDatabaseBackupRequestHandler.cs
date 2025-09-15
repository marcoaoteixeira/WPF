using System.IO;
using System.IO.Compression;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.Results;
using Nameless.WPF.Client.Sqlite.Internals;
using Nameless.WPF.Client.Sqlite.Resources;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public class PerformDatabaseBackupRequestHandler : IRequestHandler<PerformDatabaseBackupRequest, PerformDatabaseBackupResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<PerformDatabaseBackupRequestHandler> _logger;

    public PerformDatabaseBackupRequestHandler(IFileSystem fileSystem, INotificationService notificationService, TimeProvider timeProvider, ILogger<PerformDatabaseBackupRequestHandler> logger) {
        _fileSystem = Guard.Against.Null(fileSystem);
        _notificationService = Guard.Against.Null(notificationService);
        _timeProvider = Guard.Against.Null(timeProvider);
        _logger = Guard.Against.Null(logger);
    }

    public async Task<PerformDatabaseBackupResponse> HandleAsync(PerformDatabaseBackupRequest request, CancellationToken cancellationToken) {
        var databaseDataBackupResult = await ExecuteDatabaseDataBackupAsync(cancellationToken);

        if (databaseDataBackupResult.IsError) {
            return PerformDatabaseBackupResponse.Failure(databaseDataBackupResult.AsError.Description);
        }

        var backupRelativeFilePath = databaseDataBackupResult.AsResult ??
                                     throw new InvalidOperationException(Exceptions.PerformDatabaseBackupRequestHandler_InvalidOperationException_MissingBackupFilePath);

        var prepareBackupFileResult = await PrepareBackupFileAsync(backupRelativeFilePath, cancellationToken);

        return await prepareBackupFileResult.Match(
            onResult: OnSuccess,
            onError: OnFailure
        );
    }

    private static async Task<SqliteConnection> CreateSqliteConnectionAsync(string filePath, CancellationToken cancellationToken) {
        var connStr = string.Format(Constants.Database.CONN_STR_PATTERN, filePath);
        var dbConnection = new SqliteConnection(connStr);

        await dbConnection.OpenAsync(cancellationToken)
                          .SuppressContext();

        return dbConnection;
    }

    private async Task<Result<string>> ExecuteDatabaseDataBackupAsync(CancellationToken cancellationToken) {
        SqliteConnection? backupDbConnection = null;
        SqliteConnection? sourceDbConnection = null;

        try {
            await _notificationService.PublishAsync(PerformDatabaseBackupNotification.BackupDatabaseDataStarting())
                                      .SuppressContext();

            var sourceFilePath = _fileSystem.GetFullPath(Constants.Database.DATABASE_FILE_NAME);
            sourceDbConnection = await CreateSqliteConnectionAsync(sourceFilePath, cancellationToken)
                .SuppressContext();

            var now = _timeProvider.GetUtcNow();
            var backupFileName = string.Format(Constants.Database.Backup.FILE_NAME_PATTERN, now);

            // Ensure backup directory exists.
            _fileSystem.GetDirectory(Constants.Database.Backup.DIRECTORY_NAME).Create();

            var backupRelativeFilePath = Path.Combine(Constants.Database.Backup.DIRECTORY_NAME, backupFileName);
            var backupFilePath = _fileSystem.GetFullPath(backupRelativeFilePath);
            backupDbConnection = await CreateSqliteConnectionAsync(backupFilePath, cancellationToken).SuppressContext();

            sourceDbConnection.BackupDatabase(backupDbConnection);

            await _notificationService.PublishAsync(PerformDatabaseBackupNotification.BackupDatabaseDataFinish())
                                      .SuppressContext();

            return backupRelativeFilePath;
        }
        catch (Exception ex) {
            _logger.ExecuteDatabaseDataBackupFailure(ex);

            var failure = PerformDatabaseBackupNotification.BackupDatabaseDataFailure(ex.Message);
            await _notificationService.PublishAsync(failure)
                                      .SuppressContext();

            return Error.Failure(failure.Message);
        }
        finally {
            if (backupDbConnection is not null) {
                await backupDbConnection.CloseAsync()
                                        .SuppressContext();
                await backupDbConnection.DisposeAsync()
                                        .SuppressContext();
            }

            if (sourceDbConnection is not null) {
                await sourceDbConnection.CloseAsync()
                                        .SuppressContext();
                await sourceDbConnection.DisposeAsync()
                                        .SuppressContext();
            }
        }
    }

    private async Task<Result<string>> PrepareBackupFileAsync(string backupRelativeFilePath, CancellationToken cancellationToken) {
        try {
            await _notificationService.PublishAsync(PerformDatabaseBackupNotification.PrepareBackupFileStarting())
                                      .SuppressContext();

            var backupFileStream = _fileSystem.GetFile(backupRelativeFilePath)
                                              .Open();

            var compressRelativeFilePath = $"{backupRelativeFilePath}.gz";
            await using var compressFileStream = _fileSystem.GetFile(compressRelativeFilePath)
                                                            .Open();

            await using var gzipStream = new GZipStream(compressFileStream, CompressionLevel.Optimal);

            await backupFileStream.CopyToAsync(gzipStream, cancellationToken);

            // Force backup file stream dispose so we can delete the file
            await backupFileStream.DisposeAsync();

            _fileSystem.GetFile(backupRelativeFilePath)
                       .Delete();

            await _notificationService.PublishAsync(PerformDatabaseBackupNotification.PrepareBackupFileFinish())
                                      .SuppressContext();

            return _fileSystem.GetFullPath(compressRelativeFilePath);
        }
        catch (Exception ex) {
            _logger.PrepareBackupFileFailure(ex);

            var failure = PerformDatabaseBackupNotification.PrepareBackupFileFailure(ex.Message);

            await _notificationService.PublishAsync(failure)
                                      .SuppressContext();

            return Error.Failure(failure.Message);
        }
    }

    private async Task<PerformDatabaseBackupResponse> OnSuccess(string backupFilePath) {
        var notification = PerformDatabaseBackupNotification.Success(backupFilePath);

        await _notificationService.PublishAsync(notification)
                                  .SuppressContext();

        return PerformDatabaseBackupResponse.Success(backupFilePath);
    }

    private static Task<PerformDatabaseBackupResponse> OnFailure(Error error) {
        return Task.FromResult(PerformDatabaseBackupResponse.Failure(error.Description));
    }
}