namespace WorkChronicle
{
    using WorkChronicle.Common.Helpers;

    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();

            var lang = Preferences.Get("AppLanguage", "en");
            LocalizationHelper.SetCulture(lang);

            MainPage = appShell;
        }

    }
}
