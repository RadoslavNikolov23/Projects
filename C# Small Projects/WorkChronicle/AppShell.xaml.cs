namespace WorkChronicle
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("PickerDateView", typeof(PickerDateView));
            Routing.RegisterRoute("ScheduleView", typeof(ScheduleView));
            Routing.RegisterRoute("CompensateShiftsView", typeof(CompensateShiftsView));


        }

    }
}
