namespace Nameless.WPF.Client.Sqlite;

internal static class Constants {
    internal static class Database {
        internal const string DATABASE_FILE_NAME = "application.db";
        internal const string CONN_STR_PATTERN = "Data Source={0};Pooling=false;";

        internal static class Backup {
            internal const string FILE_NAME_PATTERN = "{0:yyyyMMddHHmmss}.bkp";
            internal const string DIRECTORY_NAME = "application_db_bkp";
        }
    }
}
