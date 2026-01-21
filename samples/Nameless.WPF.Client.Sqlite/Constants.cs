namespace Nameless.WPF.Client.Sqlite;

internal static class Constants {
    internal static class Application {
        internal static class Backup {
            internal const string FILE_NAME_PATTERN = "{0:yyyyMMddHHmmss}.app";
            internal const string DIRECTORY_NAME = "backup";
            internal const string FILE_EXTENSION = ".bkp";
        }
    }

    internal static class Index {
        internal static class Backup {
            internal const string FILE_NAME_PATTERN = "{0:yyyyMMddHHmmss}.index";
        }

        internal const string DIRECTORY_NAME = "index";
        internal const string NAME = "8af2e94c8dba4066815629da465e8182";
    }

    internal static class Database {
        internal static class Backup {
            internal const string FILE_NAME_PATTERN = "{0:yyyyMMddHHmmss}.db";
        }

        internal const string DIRECTORY_NAME = "database";
        internal const string NAME = "app.db";
        internal const string CONN_STR_PATTERN = "Data Source={0};Pooling=false;";
    }
}
