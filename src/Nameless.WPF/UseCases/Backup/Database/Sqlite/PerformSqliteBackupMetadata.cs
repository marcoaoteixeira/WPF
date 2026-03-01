using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

public readonly record struct PerformSqliteBackupMetadata(string BackupFilePath) {
    [MemberNotNullWhen(returnValue: true, nameof(BackupFilePath))]
    public bool IsBackupFileAvailable => !string.IsNullOrWhiteSpace(BackupFilePath);
}