namespace WorkChronicle.ViewModels
{
    public partial class PropertyViewModel:BaseViewModel
    {
        [RelayCommand]
        private async Task OpenLanguagePicker()
        {
            string action = await Application
                                    .Current!
                                    .MainPage!
                                    .DisplayActionSheet(AppResources.SelectLanguage,
                                                        "Cancel",
                                                        null,
                                                        "English", "Bulgarian");

            if (action == "English")
                await ChangeLanguageAsync("en");
            else if (action == "Bulgarian")
                await ChangeLanguageAsync("bg");
        }

        private async Task ChangeLanguageAsync(string langCode)
        {
            LocalizationHelper.SetCulture(langCode);
            Preferences.Set("AppLanguage", langCode);

            await Application
                             .Current!
                             .MainPage!
                             .DisplayAlert(AppResources.LanguageChanged,
                                        string.Format(AppResources.TheAppLanguageHasBeenChanged, langCode),
                                        "OK");

            Application.Current.MainPage = new AppShell();
        }

        [RelayCommand]
        private async Task ReportProblem()
        {
            // Placeholder for future action
            await Application.Current!.MainPage!.DisplayAlert(
                "Report",
                "Problem reporting not implemented yet.",
                "OK");
        }
    }
}
