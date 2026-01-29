namespace Nameless.WPF;

public static class Constants {
    public static class Application {
        public const string TemporaryDirectoryName = "tmp";

        public static class Backup {
            public const string FileNamePattern = "{0:yyyyMMddHHmmss}.app";
            public const string DirectoryName = "backup";
            public const string FileExtension = ".bkp";
        }
    }

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
    }
}
