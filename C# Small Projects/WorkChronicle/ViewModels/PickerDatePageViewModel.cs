namespace WorkChronicle.ViewModels
{
    public partial class PickerDatePageViewModel : BaseViewModel
    {
        private ISchedule<IShift> schedule;

        private readonly IEngine engine;

        [ObservableProperty]
        private DateTime selectedStartDate;

        [ObservableProperty]
        private string selectedSchedule = "";

        [ObservableProperty]
        private string resultsMessage = "";


        public PickerDatePageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
            this.engine = new Engine();
            this.selectedStartDate = DateTime.Now;
        }


        public ObservableCollection<string> WorkSchedules { get; } = new()
        {
            "Day-Night",
            "Day-Day",
            "Day-Night-Night"
        };


        [RelayCommand]
        private async Task CalculateShifts()
        {
            DateTime startDate = SelectedStartDate.Date;

            string[] cycle= await ValidateSchedule();

            ISchedule<IShift> tempSchedule = this.engine.CalculateShifts(startDate, cycle);

            foreach (var shift in tempSchedule.WorkSchedule)
            {
                await schedule.AddShift(shift);
            }

            await Shell.Current.GoToAsync(nameof(SchedulePage));
        }


        
        private async Task<string[]> ValidateSchedule()
        {
            await Task.Delay(100);

            if (string.IsNullOrEmpty(SelectedSchedule))
            {
                ResultsMessage = "Please select a work schedule first.";
                return Array.Empty<string>();
            }

            string[] cycle = SelectedSchedule.Split('-');
            if (cycle.Length == 0)
            {
                ResultsMessage = "Something went wrong";
                return Array.Empty<string>();
            }


            return cycle;
        }
    }
}
