namespace WorkChronicle
{
    public partial class MainPage : INotifyPropertyChanged
    {
       // private readonly IEngine engine; //TODO To Remove

        public MainPage(IEngine engine)
        {
            InitializeComponent();
            BindingContext = this;
            // this.engine = engine;  //TODO To Remove
        }


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
