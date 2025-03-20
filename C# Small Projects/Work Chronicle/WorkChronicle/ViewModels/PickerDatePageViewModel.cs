namespace WorkChronicle.ViewModels
{


    public partial class PickerDatePageViewModel : BaseViewModel
    {
        private ISchedule<IShift> schedule;

        private readonly IEngine<ISchedule<IShift>> engine;

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



        public PickerDatePageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
            this.engine = new Engine();
            this.selectedStartDate = DateTime.Now;
            this.selectedFirstShift = WorkShift[0];
            this.dayShiftStartTime = new TimeSpan(07, 00, 00);
            this.nightShiftStartTime = new TimeSpan(19, 00, 00);
            this.totalShiftHours = 12;
        }

        public ObservableCollection<string> WorkSchedules { get; } = new()
        {
            "Day24Hour",
            "Day-Day",
            "Day-Night",
            "Day-Night-Night"
        };

        //public ObservableCollection<string> DisplayWorkSchedules
        //{
        //    get
        //    {
        //        return new ObservableCollection<string>(WorkSchedules.Select(s => s == "Day" ? "Day24Hour" : s));
        //    }
        //}

        public ObservableCollection<string> WorkShift { get; } = new()
        {
            "DayShift",
            "NightShift",
        };


        [RelayCommand]
        private async Task CalculateShifts()
        {
            DateTime startDate = SelectedStartDate.Date;

            string[] cycle = await ValidateSchedule();


            ShiftConfiguration shiftConfiguration = new ShiftConfiguration(this.DayShiftStartTime.Hours, this.NightShiftStartTime.Hours, this.TotalShiftHours);
            ScheduleConfiguration scheduleConfiguration = new ScheduleConfiguration(startDate, cycle, this.SelectedFirstShift, shiftConfiguration);


            ISchedule<IShift> tempSchedule = await this.engine.CalculateShifts(scheduleConfiguration);

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
