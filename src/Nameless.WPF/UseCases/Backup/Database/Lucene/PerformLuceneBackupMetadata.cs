using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

public readonly record struct PerformLuceneBackupMetadata(string BackupFilePath) {
    [MemberNotNullWhen(returnValue: true, nameof(BackupFilePath))]
    public bool IsBackupFileAvailable => !string.IsNullOrWhiteSpace(BackupFilePath);
}