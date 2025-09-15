using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public record PerformDatabaseBackupResponse {
    public string? BackupFilePath { get; }

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: true, nameof(BackupFilePath))]
    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private PerformDatabaseBackupResponse(string? backupFilePath, string? error) {
        BackupFilePath = backupFilePath;
        Error = error;
    }

    public static PerformDatabaseBackupResponse Success(string backupFilePath) {
        return new PerformDatabaseBackupResponse(backupFilePath, error: null);
    }

    public static PerformDatabaseBackupResponse Failure(string error) {
        return new PerformDatabaseBackupResponse(backupFilePath: null, error);
    }
}