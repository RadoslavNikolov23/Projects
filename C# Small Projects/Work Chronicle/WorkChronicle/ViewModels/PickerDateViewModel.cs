namespace WorkChronicle.ViewModels
{
    public partial class PickerDateViewModel : BaseViewModel
    {
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private DateTime selectedStartDate;

        [ObservableProperty]
        private string selectedSchedule = "";

        [ObservableProperty]
        private string resultsMessage = "";

        [ObservableProperty]
        private string selectedFirstShift = "";

        [ObservableProperty]
        private TimeSpan dayShiftStartTime;

        [ObservableProperty]
        private TimeSpan nightShiftStartTime;

        [ObservableProperty]
        private int totalShiftHours;

        public List<int> ShiftDurations { get; } = new() { 4, 6, 8, 10, 12, 24 };

        public ObservableCollection<string> WorkSchedules { get; } = new()
        {
            "Day24Hour",
            "Day-Day",
            "Day-Night",
            "Day-Night-Night"
        };

        public ObservableCollection<string> WorkShift { get; } = new()
        {
            "DayShift",
            "NightShift",
        };

        public PickerDateViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
            this.schedule.WorkSchedule.Clear(); // Clear the current schedule

            this.selectedStartDate = DateTime.Now;
            this.selectedFirstShift = WorkShift[0];
            this.dayShiftStartTime = new TimeSpan(07, 00, 00);
            this.nightShiftStartTime = new TimeSpan(19, 00, 00);
            this.totalShiftHours = 12;
        }

        [RelayCommand]
        private async Task CalculateShifts()
        {
            DateTime startDate = SelectedStartDate.Date;

            string[] cycle = await ValidateSchedule();


            if (this.TotalShiftHours <= 0)
            {
                await ShowPopupMessage("Error", "The total shift hours must be a positive number!");
                return;
            }

            if ((this.SelectedFirstShift != ShiftType.DayShift.ToString() && this.SelectedFirstShift != ShiftType.NightShift.ToString())
                 || string.IsNullOrEmpty(this.SelectedFirstShift))
            {
                await ShowPopupMessage("Error", "Please select a valid work shift.");
                return;
            }

            if (cycle.Length == 1 && this.SelectedFirstShift == ShiftType.NightShift.ToString())
            {
                await ShowPopupMessage("Error", $"In a 24HourDay schedule, {ShiftType.NightShift.ToString()} can't be select as first shift, please select {ShiftType.DayShift.ToString()}");
                return;
            }
            else if ((cycle.Length == 2 && cycle[1] == ShiftType.DayShift.ToString()) && this.SelectedFirstShift == ShiftType.NightShift.ToString())
            {
                await ShowPopupMessage("Error", $"In a Day-Day schedule, {ShiftType.NightShift.ToString()} can't be select as first shift, please select {ShiftType.DayShift.ToString()}");
                return;
            }

            ShiftConfiguration shiftConfiguration = new(this.DayShiftStartTime.Hours,
                                                        this.NightShiftStartTime.Hours,
                                                        this.TotalShiftHours);

            ScheduleConfiguration scheduleConfiguration = new(startDate,
                                                               cycle,
                                                               this.SelectedFirstShift,
                                                               shiftConfiguration);

            IEngine<ISchedule<IShift>> engine = new Engine();
            ISchedule<IShift> tempSchedule = await engine.CalculateShifts(scheduleConfiguration);

            if(tempSchedule == null || tempSchedule.WorkSchedule.Count==0)
            {
                await ShowPopupMessage("Error", "An error occurred while calculating the shifts. Try again!");
                return;
            }

            foreach (var shift in tempSchedule.WorkSchedule)
            {
                await schedule.AddShift(shift);
            }

            //Check if the schedule is transferred correctly to the SchedulePage
            await Shell.Current.GoToAsync(nameof(SchedulePage));
        }

        private async Task<string[]> ValidateSchedule()
        {
            if (string.IsNullOrEmpty(this.SelectedSchedule))
            {
                await ShowPopupMessage("Error", "Please select a work schedule first.");
                return Array.Empty<string>();
            }

            string[] cycle = SelectedSchedule.Split('-');

            if (cycle.Length == 0)
            {
                await ShowPopupMessage("Error", "Please select a work schedule first.");
                return Array.Empty<string>();
            }

            return cycle;
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
