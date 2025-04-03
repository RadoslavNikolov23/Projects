namespace WorkChronicle.Data.Connection
{

    public static class DatabaseConfiguration
    {
        private const string DatabaseFilename = "WorkChronicleDatabase.db3";

        public const SQLite.SQLiteOpenFlags Flags =
                                        SQLite.SQLiteOpenFlags.ReadWrite |
                                        SQLite.SQLiteOpenFlags.Create |
                                        SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    }

}
