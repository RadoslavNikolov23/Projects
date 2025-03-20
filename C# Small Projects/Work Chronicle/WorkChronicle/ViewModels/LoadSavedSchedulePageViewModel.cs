namespace WorkChronicle.ViewModels
{
    using WorkChronicle.Structure.Models.Contracts;
    using WorkChronicle.Structure.Repository.Contracts;

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
        }
    }
}
