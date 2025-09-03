using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.Database.Backup;

public record PerformDatabaseBackupRequest : IRequest<PerformDatabaseBackupResponse>;