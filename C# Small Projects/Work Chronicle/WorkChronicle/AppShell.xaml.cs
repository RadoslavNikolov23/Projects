﻿namespace WorkChronicle
{

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(PickerDatePage), typeof(PickerDatePage));
            Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
            Routing.RegisterRoute(nameof(CompensateShiftsPage), typeof(CompensateShiftsPage));
            Routing.RegisterRoute(nameof(PropertiePage), typeof(PropertiePage));
        }


        private async void OnExitClicked(object sender, EventArgs e)
        {
            // Exit the app
            Application.Current!.Quit();
        }

    }
}
