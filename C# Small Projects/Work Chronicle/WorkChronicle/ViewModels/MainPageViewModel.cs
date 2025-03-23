namespace WorkChronicle.ViewModels
{
    public partial class MainPageViewModel: BaseViewModel
    {
        
        public MainPageViewModel()
        {

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
