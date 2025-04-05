namespace WorkChronicle.Views
{
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage(SchedulePageViewModel schedulePageViewModel)
        {
            InitializeComponent();
            BindingContext = schedulePageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is SchedulePageViewModel sv)
            {
                await sv.RefreshThePage();

            }
        }
    }
}