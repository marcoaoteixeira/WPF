using Nameless.IO.FileSystem;

namespace Nameless.WPF.Extensions;

public static class FileSystemExtensions {
    extension(IFileSystem self) {
        public string EnsureApplicationTemporaryDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.Application.TemporaryDirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string EnsureApplicationBackupDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.Application.Backup.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string EnsureSqliteDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.Sqlite.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string EnsureLuceneDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.Lucene.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string EnsureSystemUpdateDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.SystemUpdate.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }
    }
}
