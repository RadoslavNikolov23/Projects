namespace WorkChronicle.Views
{
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage(ScheduleEditViewModel schedulePageViewModel)
        {
            InitializeComponent();
            BindingContext = schedulePageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ScheduleEditViewModel sv)
            {
                await sv.RefreshThePage();

            }
        }
    }
}