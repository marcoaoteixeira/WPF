using Nameless.Mediator.Requests;

namespace Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;

public record PerformDatabaseBackupRequest : IRequest<PerformDatabaseBackupResponse>;