using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Nameless.IO.FileSystem;
using Nameless.Mediator.Requests;
using Nameless.ObjectModel;
using Nameless.Results;
using Nameless.WPF.Notifications;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

public class PerformSqliteBackupRequestHandler : IRequestHandler<PerformSqliteBackupRequest, PerformSqliteBackupResponse> {
    private readonly IFileSystem _fileSystem;
    private readonly IPushNotification _pushNotification;
    private readonly ILogger<PerformSqliteBackupRequestHandler> _logger;

    public PerformSqliteBackupRequestHandler(
        IFileSystem fileSystem,
        IPushNotification pushNotification,
        ILogger<PerformSqliteBackupRequestHandler> logger) {
        _fileSystem = fileSystem;
        _pushNotification = pushNotification;
        _logger = logger;
    }

    public async Task<PerformSqliteBackupResponse> HandleAsync(PerformSqliteBackupRequest request, CancellationToken cancellationToken) {
        _fileSystem.EnsureTemporaryDirectoryExistence();

        await _pushNotification.NotifyStartingAsync()
                               .SkipContextSync();

        var databaseBackupResult = await ExecuteDatabaseBackupAsync(
            request.BackupDate,
            cancellationToken
        ).SkipContextSync();

        if (databaseBackupResult.Failure) {
            return databaseBackupResult.Errors[0];
        }

        await _pushNotification.NotifySuccessAsync(databaseBackupResult.Value)
                               .SkipContextSync();

        return new PerformSqliteBackupMetadata(
            BackupFilePath: databaseBackupResult.Value
        );
    }

    private async Task<Result<string>> ExecuteDatabaseBackupAsync(DateTimeOffset backupDate, CancellationToken cancellationToken) {
        SqliteConnection? sourceDbConnection = null;
        SqliteConnection? destinationDbConnection = null;

        try {
            var sourceFilePath = GetSourceFilePath();
            sourceDbConnection = await CreateSqliteConnectionAsync(sourceFilePath, cancellationToken).SkipContextSync();

            var destinationFilePath = GetDestinationFilePath();
            destinationDbConnection = await CreateSqliteConnectionAsync(destinationFilePath, cancellationToken).SkipContextSync();

            sourceDbConnection.BackupDatabase(destinationDbConnection);

            return destinationFilePath;
        }
        catch (Exception ex) {
            _logger.ExecuteDatabaseBackupFailure(ex);

            await _pushNotification.NotifyFailureAsync(ex.Message)
                                   .SkipContextSync();

            return Error.Failure(ex.Message);
        }
        finally {
            if (destinationDbConnection is not null) {
                await destinationDbConnection.CloseAsync().SkipContextSync();
                await destinationDbConnection.DisposeAsync();
            }

            if (sourceDbConnection is not null) {
                await sourceDbConnection.CloseAsync().SkipContextSync();
                await sourceDbConnection.DisposeAsync();
            }
        }

        string GetSourceFilePath() {
            return _fileSystem.GetFullPath(
                Path.Combine(
                    Constants.FolderStructure.DatabasesDirectoryName,
                    Constants.Sqlite.FileName
                )
            );
        }

        static async Task<SqliteConnection> CreateSqliteConnectionAsync(string filePath, CancellationToken cancellationToken) {
            var connStr = string.Format(Constants.Sqlite.ConnStrPattern, filePath);
            var dbConnection = new SqliteConnection(connStr);

            await dbConnection.OpenAsync(cancellationToken)
                              .SkipContextSync();

            return dbConnection;
        }

        string GetDestinationFilePath() {
            var fileName = string.Format(
                Constants.BackupDefinitions.FileNamePattern,
                backupDate,
                Constants.BackupDefinitions.SqliteExtension
            );

            return _fileSystem.GetFullPath(
                Path.Combine(
                    Constants.FolderStructure.TemporaryDirectoryName,
                    fileName
                )
            );
        }
    }
}