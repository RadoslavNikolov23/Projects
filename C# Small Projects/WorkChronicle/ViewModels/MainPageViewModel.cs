namespace WorkChronicle.ViewModels
{
    public partial class MainPageViewModel: BaseViewModel
    {
        
        private ISchedule<IShift> schedule;
        public MainPageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;   
        }

        [RelayCommand]
        private async Task ViewSavedSchedules()
        {
            //await Navigation.PushAsync(new ScheduleView(this.schedule));
            await Shell.Current.GoToAsync($"LoadSavedSchedulePage");
        }

        [RelayCommand]
        private async Task PickNewSchedule()
        {
            //await Navigation.PushAsync(new PickerDateView(engine));
            await Shell.Current.GoToAsync($"PickerDatePage");
        }

        [RelayCommand]
        private void ExitApp()
        {
            Application.Current!.Quit();
        }
    }
}
