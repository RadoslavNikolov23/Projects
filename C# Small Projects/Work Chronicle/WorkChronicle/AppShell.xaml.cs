namespace WorkChronicle
{

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(PickerDatePage), typeof(PickerDatePage));
            Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
            Routing.RegisterRoute(nameof(LoadSchedulePage), typeof(LoadSchedulePage));
            Routing.RegisterRoute(nameof(PropertyPage), typeof(PropertyPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        }

        private async void OnHelpClicked(object sender, EventArgs e)
        {
            var url = "https://github.com/RadoslavNikolov23"; // <-- Change to repo folder !!!!!
            await Launcher.Default.OpenAsync(url);
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            await Task.Delay(100); // Optional delay for smoother exit experience
            // Exit the app
            Application.Current!.Quit();
        }

    }
}
