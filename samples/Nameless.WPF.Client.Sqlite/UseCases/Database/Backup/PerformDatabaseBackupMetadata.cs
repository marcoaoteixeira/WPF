using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public readonly record struct PerformDatabaseBackupMetadata(string? FilePath) {
    [MemberNotNullWhen(returnValue: true, nameof(FilePath))]
    public bool IsBackupFileAvailable => !string.IsNullOrWhiteSpace(FilePath);
}