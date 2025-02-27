namespace WorkChronicle.ViewModels
{
    public partial class LoadSavedSchedulePageViewModel:BaseViewModel
    {
        private readonly ISchedule<IShift> schedule;
        public LoadSavedSchedulePageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
        }

        [RelayCommand]
        private async Task LoadSaved()
        {
            await Shell.Current.GoToAsync(nameof(SchedulePage));
           // await Shell.Current.GoToAsync("//SchedulePage");
            //await Shell.Current.GoToAsync("//SchedulePage", true);


            // await Navigation.PushAsync(new ScheduleView(schedule));

        }
    }
}
