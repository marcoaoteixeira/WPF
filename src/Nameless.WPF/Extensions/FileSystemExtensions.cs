using System.IO;
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

            return directory.Name;
        }

        public string GetApplicationBackupFilePath(DateTimeOffset backupDate) {
            var directory = self.EnsureApplicationBackupDirectoryExistence();

            return Path.Combine(
                directory,
                string.Format(Constants.Application.Backup.FileNamePattern, backupDate)
            );
        }

        public string EnsureDatabaseDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.Database.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string GetDatabaseBackupFilePath(DateTimeOffset backupDate) {
            var directory = self.EnsureDatabaseDirectoryExistence();

            return Path.Combine(
                directory,
                string.Format(Constants.Database.Backup.FileNamePattern, backupDate)
            );
        }

        public string EnsureDocumentIndexDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.DocumentIndex.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string GetDocumentIndexBackupFilePath(DateTimeOffset backupDate) {
            var directory = self.EnsureDocumentIndexDirectoryExistence();

            return Path.Combine(
                directory,
                string.Format(Constants.DocumentIndex.Backup.FileNamePattern, backupDate)
            );
        }

        public string EnsureSystemUpdateDirectoryExistence() {
            var directory = self.GetDirectory(
                Constants.SystemUpdate.DirectoryName
            );

            directory.Create();

            return directory.Path;
        }

        public string GetSystemUpdateFilePath(string version) {
            var directory = self.EnsureSystemUpdateDirectoryExistence();

            return Path.Combine(
                directory,
                string.Format(Constants.SystemUpdate.FileNamePattern, version)
            );
        }

        public string GetSystemUpdateFilePath(Version version) {
            return self.GetSystemUpdateFilePath(
                $"v{version.Major}.{version.Minor}.{version.Build}"
            );
        }
    }
}
