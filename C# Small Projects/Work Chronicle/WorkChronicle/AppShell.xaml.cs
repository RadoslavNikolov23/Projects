namespace WorkChronicle
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("SchedulePage", typeof(SchedulePage));
            Routing.RegisterRoute("PickerDatePage", typeof(PickerDatePage));
            Routing.RegisterRoute("LoadSavedSchedulePage", typeof(LoadSavedSchedulePage));
            Routing.RegisterRoute("CompensateShiftsPage", typeof(CompensateShiftsPage));
        }

    }
}
