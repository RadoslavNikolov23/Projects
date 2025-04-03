namespace WorkChronicle.ViewModels
{
    public partial class CalendarDay : ObservableObject
    {
        [ObservableProperty]
        public string day;

        [ObservableProperty]
        public string backgroundColor;
    }

    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly WorkScheduleRepositoryDB scheduleRepositoryDB;

        private readonly ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<string> scheduleNames;

        [ObservableProperty]
        private string selectedScheduleName = "";

        [ObservableProperty]
        private string selectedMonthYear;

        public ObservableCollection<CalendarDay> CalendarDays { get; set; } = new();

        public MainPageViewModel(ISchedule<IShift> schedule, WorkScheduleRepositoryDB scheduleRepositoryDB)
        {
            this.schedule = schedule;
            this.scheduleRepositoryDB = scheduleRepositoryDB;
            scheduleNames = new ObservableCollection<string>();
            //LoadLastSchedule();
            _ = LoadCalendar(2025, 3); //Change the year and month to be from the DB

        }

        public async Task LoadCalendar(int year, int month)
        {
            CalendarDays.Clear();

            //Check scheduleRepositoryDB is there a way to get the name of the table!!!!
            selectedMonthYear = $"Work Schedule for {month} {year} ";

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                ShiftType shiftType = GetShiftForDay(year, month, day);
                
                string color = shiftType switch
                {
                    ShiftType.DayShift => "LightGreen",
                    ShiftType.NightShift => "LightCoral",
                    _ => "White" // RestDays
                };

                CalendarDays.Add(new CalendarDay
                {
                   Day = $"{day} {shiftType.ToString()}",
                   BackgroundColor = color
                });
            }
        }

        private ShiftType GetShiftForDay(int year, int month, int day)
        {
            // Mock Shift Data (Replace with Database Data)
            return day % 5 == 0 ? ShiftType.RestDay : (day % 2 == 0 ? ShiftType.DayShift : ShiftType.NightShift);
        }


        [RelayCommand]
        public async Task LoadScheduleNamesAsync()
        {
            var schedules = await scheduleRepositoryDB.GetSchedules();
            ScheduleNames.Clear();
            foreach (var s in schedules)
                ScheduleNames.Add(s.ScheduleName);

        }



        //private void LoadLastSchedule()
        //{
        //    // Fetch the most recent schedule from the database
        //    LastSchedule = _scheduleRepository.GetLastSchedule();

        //    if (LastSchedule != null)
        //    {
        //        GenerateDays(LastSchedule.Year, LastSchedule.Month, LastSchedule.Shifts);
        //    }
        //}

        //private Task GenerateDays()
        //{
        //    Days.Clear();
        //    var shift = schedule.WorkSchedule.First();
        //    int daysInMonth = DateTime.DaysInMonth(shift.Year, shift.Month);
        //    var shiftDays = schedule.WorkSchedule.Where(s => s.Year == shift.Year && s.Month == shift.Year)
        //                          .Select(s => s.Day)
        //                          .Distinct();

        //    for (int day = 1; day <= daysInMonth; day++)
        //    {
        //        Days.Add(new DayItem
        //        {
        //            Day = day,
        //            HasShift = shiftDays.Contains(day)
        //        });
        //    }

        //    return Task.CompletedTask;
        //}


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
