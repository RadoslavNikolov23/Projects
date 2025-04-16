namespace WorkChronicle.Common.Helpers
{
    public static class LocalizationHelper
    {
        public static void SetCulture(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static string CurrentCulture => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }
}
