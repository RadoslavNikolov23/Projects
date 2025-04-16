namespace WorkChronicle.Common.Providers
{

    public class CulturalProvider
    {
        public static string GetMonthName(int month)
        {
            try
            {
                var culture = Thread.CurrentThread.CurrentUICulture;
                return culture.DateTimeFormat.GetMonthName(month);
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
