using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.Backup.Application;

public readonly record struct PerformApplicationBackupMetadata(string BackupFilePath) {
    [MemberNotNullWhen(returnValue: true, nameof(BackupFilePath))]
    public bool IsBackupFileAvailable => !string.IsNullOrWhiteSpace(BackupFilePath);
}