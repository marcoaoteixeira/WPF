using Nameless.ObjectModel;
using Nameless.Results;

namespace Nameless.WPF.UseCases.Backup.Database.Sqlite;

public class PerformSqliteBackupResponse : Result<PerformSqliteBackupMetadata> {
    private PerformSqliteBackupResponse(PerformSqliteBackupMetadata value, Error[] errors)
        : base(value, errors) { }

    public static implicit operator PerformSqliteBackupResponse(PerformSqliteBackupMetadata value) {
        return new PerformSqliteBackupResponse(value, errors: []);
    }

    public static implicit operator PerformSqliteBackupResponse(Error error) {
        return new PerformSqliteBackupResponse(value: default, errors: [error]);
    }
}