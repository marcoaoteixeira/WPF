using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

public class PerformLuceneBackupRequest : IRequest<PerformLuceneBackupResponse> {
    public required DateTimeOffset BackupDate { get; init; }
}