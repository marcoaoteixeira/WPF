using Nameless.ObjectModel;
using Nameless.Results;

namespace Nameless.WPF.UseCases.Backup.Database.Lucene;

public class PerformLuceneBackupResponse : Result<PerformLuceneBackupMetadata> {
    private PerformLuceneBackupResponse(PerformLuceneBackupMetadata value, Error[] errors)
        : base(value, errors) { }

    public static implicit operator PerformLuceneBackupResponse(PerformLuceneBackupMetadata value) {
        return new PerformLuceneBackupResponse(value, errors: []);
    }

    public static implicit operator PerformLuceneBackupResponse(Error error) {
        return new PerformLuceneBackupResponse(value: default, errors: [error]);
    }
}