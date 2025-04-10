namespace WorkChronicle.Views
{
    public partial class LoadSchedulePage : ContentPage
    {
        public LoadSchedulePage(LoadScheduleViewModel loadScheduleViewModel)
        {
            InitializeComponent();
            BindingContext = loadScheduleViewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is LoadScheduleViewModel mv)
            {
                await mv.RefreshThePage();

            }
        }

    }
}
