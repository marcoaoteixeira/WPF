namespace Nameless.WPF;

public static class Constants {
    public static class FolderStructure {
        public const string BackupsDirectoryName = "backups";
        public const string DatabasesDirectoryName = "databases";
        public const string TemporaryDirectoryName = "tmp";
        public const string UpdatesDirectoryName = "updates";
    }

    public static class BackupDefinitions {
        public const string FileNamePattern = "{0:yyyyMMddHHmmss}{1}";

        public const string ApplicationExtension = ".bkapp";
        public const string SqliteExtension = ".dat";
        public const string LuceneExtension = ".idx";
    }

<<<<<<< Updated upstream
    public static class Database {
        public const string FileName = "app.db";
        public const string DirectoryName = "databases";
        public const string ConnectionStringPattern = "Data Source={0};Pooling=false;";
        
        public static class Backup {
            public const string FileNamePattern = "{0:yyyyMMddHHmmss}.db";
        }
    }

    public static class DocumentIndex {
        public const string Name = "4170706C69636174696F6E204C7563656E6520496E646578";
        public const string DirectoryName = "indexes";

        public static class Backup {
            public const string FileNamePattern = "{0:yyyyMMddHHmmss}.index";
        }
    }

    public static class SystemUpdate {
        public const string DirectoryName = "updates";
        public const string FileNamePattern = "{0}.update.zip";
=======
    public static class Sqlite {
        public const string FileName = "sqlite.db";
        public const string ConnStrPattern = "Data Source={0};Pooling=false;";
    }

    public static class Lucene {
        public const string UniqueIndexName = "78213cbeef85474686c80570279befd5";
>>>>>>> Stashed changes
    }
}
