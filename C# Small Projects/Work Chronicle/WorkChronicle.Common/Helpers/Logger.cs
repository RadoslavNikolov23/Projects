namespace WorkChronicle.Common.Helpers
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(FileSystem.AppDataDirectory, "app_log.txt");

        public static async Task LogAsync(Exception ex, string context = "")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n{ex}\n\n";
                await File.AppendAllTextAsync(LogFilePath, logEntry);
            }
            catch
            {
                // Fails silently to avoid recursive logging problems
            }
        }

        public static string GetLogFilePath() => LogFilePath;

        public static async Task<string> ReadLogAsync()
        {
            try
            {
                if (File.Exists(LogFilePath))
                    return await File.ReadAllTextAsync(LogFilePath);

                return "No logs found.";
            }
            catch
            {
                return "Failed to read logs.";
            }
        }

        public static void ClearLog()
        {
            try
            {
                if (File.Exists(LogFilePath))
                    File.Delete(LogFilePath);
            }
            catch { }
        }
    }
}
