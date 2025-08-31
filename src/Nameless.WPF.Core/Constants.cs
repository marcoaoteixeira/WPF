namespace Nameless.WPF;

public static class Constants {
    public static class Application {
        public const string Name = "ClientApp";
        public const string LogFileName = "application.log";
    }

    public static class Database {
        public const string DatabaseFileName = "application.db";
        public const string ConnStrPattern = "Data Source={0};Pooling=false;";

        public static class Backup {
            public const string FileNamePattern = "{0:yyyyMMddHHmmss}.bkp";
            public const string DirectoryName = "application_db_bkp";
        }
    }
}
