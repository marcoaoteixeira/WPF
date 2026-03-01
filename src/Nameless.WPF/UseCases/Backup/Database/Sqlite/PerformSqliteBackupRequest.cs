using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

public record PerformSqliteBackupRequest : IRequest<PerformSqliteBackupResponse> {
    public required DateTimeOffset BackupDate { get; init; }
}