namespace WorkChronicle.Data.Connection
{
    public static class DatabaseConfiguration
    {
        private const string DatabaseFilename = "WorkChronicleDatabase.db";

#if ANDROID
    public static string DatabasePath =>
        Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFilename);
#elif WINDOWS
    public static string DatabasePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
#elif IOS
    public static string DatabasePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseFilename);
#else
        public static string DatabasePath => DatabaseFilename; // fallback
#endif
    }
}
