namespace Nameless.WPF;

public static class Constants {
    public static class Application {
        public const string NAME = "ClientApp";
        public const string LOG_FILE_NAME = "application.log";
    }

    public static class Database {
        public const string DATABASE_FILE_NAME = "application.db";
        public const string CONN_STR_PATTERN = "Data Source={0};Pooling=false;";

        public static class Backup {
            public const string FILE_NAME_PATTERN = "{0:yyyyMMddHHmmss}.bkp";
            public const string DIRECTORY_NAME = "application_db_bkp";
        }
    }
}
