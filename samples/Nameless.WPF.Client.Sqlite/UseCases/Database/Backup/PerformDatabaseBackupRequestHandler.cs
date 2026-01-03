using System.IO;
using System.IO.Compression;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.ObjectModel;
using Nameless.Results;
using Nameless.WPF.Client.Sqlite.Internals;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public class PerformDatabaseBackupRequestHandler : IRequestHandler<PerformDatabaseBackupRequest, PerformDatabaseBackupResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly INotificationService _notificationService;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<PerformDatabaseBackupRequestHandler> _logger;

    public PerformDatabaseBackupRequestHandler(IFileSystem fileSystem, INotificationService notificationService, TimeProvider timeProvider, ILogger<PerformDatabaseBackupRequestHandler> logger) {
        _fileSystem = fileSystem;
        _notificationService = notificationService;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<PerformDatabaseBackupResponse> HandleAsync(PerformDatabaseBackupRequest request, CancellationToken cancellationToken) {
        var databaseDataBackupResult = await ExecuteDatabaseBackupAsync(cancellationToken);

        if (!databaseDataBackupResult.Success) {
            return databaseDataBackupResult.Errors[0];
        }

        var backupRelativeFilePath = databaseDataBackupResult.Value;
        var prepareBackupFileResult = await PrepareBackupFileAsync(
            backupRelativeFilePath,
            cancellationToken
        ).SkipContextSync();

        return await prepareBackupFileResult.Match(
            onSuccess: OnSuccess,
            onFailure: errors => OnFailure(errors[0])
        );
    }

    private static async Task<SqliteConnection> CreateSqliteConnectionAsync(string filePath, CancellationToken cancellationToken) {
        var connStr = string.Format(Constants.Database.CONN_STR_PATTERN, filePath);
        var dbConnection = new SqliteConnection(connStr);

        await dbConnection.OpenAsync(cancellationToken)
                          .SkipContextSync();

        return dbConnection;
    }

    private async Task<Result<string>> ExecuteDatabaseBackupAsync(CancellationToken cancellationToken) {
        SqliteConnection? backupDbConnection = null;
        SqliteConnection? sourceDbConnection = null;

        try {
            await _notificationService.PerformDatabaseBackup_DatabaseBackup_StartingAsync()
                                      .SkipContextSync();

            var sourceFilePath = _fileSystem.GetFullPath(Constants.Database.DATABASE_FILE_NAME);
            sourceDbConnection = await CreateSqliteConnectionAsync(sourceFilePath, cancellationToken).SkipContextSync();

            var now = _timeProvider.GetUtcNow();
            var backupFileName = string.Format(Constants.Database.Backup.FILE_NAME_PATTERN, now);

            // Ensure backup directory exists.
            _fileSystem.GetDirectory(Constants.Database.Backup.DIRECTORY_NAME).Create();

            var backupRelativeFilePath = Path.Combine(Constants.Database.Backup.DIRECTORY_NAME, backupFileName);
            var backupFilePath = _fileSystem.GetFullPath(backupRelativeFilePath);
            backupDbConnection = await CreateSqliteConnectionAsync(backupFilePath, cancellationToken).SkipContextSync();

            sourceDbConnection.BackupDatabase(backupDbConnection);

            await _notificationService.PerformDatabaseBackup_DatabaseBackup_FinishAsync()
                                      .SkipContextSync();

            return backupRelativeFilePath;
        }
        catch (Exception ex) {
            _logger.ExecuteDatabaseDataBackupFailure(ex);

            await _notificationService.PerformDatabaseBackup_DatabaseBackup_FailureAsync(ex.Message)
                                      .SkipContextSync();

            return Error.Failure(ex.Message);
        }
        finally {
            if (backupDbConnection is not null) {
                await backupDbConnection.CloseAsync().SkipContextSync();
                await backupDbConnection.DisposeAsync();
            }

            if (sourceDbConnection is not null) {
                await sourceDbConnection.CloseAsync().SkipContextSync();
                await sourceDbConnection.DisposeAsync();
            }
        }
    }

    private async Task<Result<string>> PrepareBackupFileAsync(string backupRelativeFilePath, CancellationToken cancellationToken) {
        try {
            await _notificationService.PerformDatabaseBackup_PrepareBackupFile_StartingAsync()
                                      .SkipContextSync();

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

            await _notificationService.PerformDatabaseBackup_PrepareBackupFile_FinishAsync()
                                      .SkipContextSync();

            return _fileSystem.GetFullPath(compressRelativeFilePath);
        }
        catch (Exception ex) {
            _logger.PrepareBackupFileFailure(ex);

            await _notificationService.PerformDatabaseBackup_PrepareBackupFile_FailureAsync(ex.Message)
                                      .SkipContextSync();

            return Error.Failure(ex.Message);
        }
    }

    private async Task<PerformDatabaseBackupResponse> OnSuccess(string backupFilePath) {
        await _notificationService.PerformDatabaseBackup_SuccessAsync(backupFilePath)
                                  .SkipContextSync();

        return new PerformDatabaseBackupMetadata(backupFilePath);
    }

    private static Task<PerformDatabaseBackupResponse> OnFailure(Error error) {
        return Task.FromResult< PerformDatabaseBackupResponse>(error);
    }
}