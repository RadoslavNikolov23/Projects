namespace WorkChronicle.ViewModels
{
    using WorkChronicle.Structure.Models.Contracts;
    using WorkChronicle.Structure.Repository.Contracts;

    public partial class MainPageViewModel: BaseViewModel
    {
        
        private readonly ISchedule<IShift> schedule;
        public MainPageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;   
        }

        [RelayCommand]
        private async Task ViewSavedSchedules()
        {
            await Shell.Current.GoToAsync(nameof(LoadSavedSchedulePage));
        }

        [RelayCommand]
        private async Task PickNewSchedule()
        {
            await Shell.Current.GoToAsync(nameof(PickerDatePage));
        }

        [RelayCommand]
        private void ExitApp()
        {
            Application.Current!.Quit();
        }
    }
}
