
namespace WorkChronicle
{
    public partial class MainPage : INotifyPropertyChanged
    {
        private ISchedule<IShift> schedule;
        public MainPage(ISchedule<IShift> schedule)
        {
            InitializeComponent();
            this.schedule= schedule;
            BindingContext = this;
        }

        //override protected void OnAppearing()
        //{
        //    base.OnAppearing();
        //    schedule = new Schedule();
        //    BindingContext = this;

        //}


        private async void ViewSavedSchedules(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ScheduleView(this.schedule));
            await Shell.Current.GoToAsync($"LoadSavedScheduleView");
        }

        private async void PickNewSchedule(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PickerDateView(engine));
           await Shell.Current.GoToAsync($"PickerDateView");
        }

        private void ExitApp(object sender, EventArgs e)
        {
            Application.Current!.Quit();
        }


    }

}
