namespace WorkChronicle.ViewModels
{

    public partial class MainPageViewModel : BaseViewModel
    {

        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB? shiftsRepo;


        private DbSchedule? dbSchedule;

        private IList<DbShift>? dbShifts;


        private ISchedule<IShift> schedule;

        public ObservableCollection<CalendarDay> calendarDays { get; set; }

        [ObservableProperty]
        private ObservableCollection<string> scheduleNames;

        [ObservableProperty]
        private string selectedScheduleName = "";

        [ObservableProperty]
        private string calendarMonthYear = "";


      //  public MainPageViewModel(ISchedule<IShift> schedule,WorkScheduleRepositoryDB scheduleRepo, WorkShiftRepositoryDB shiftsRepo)
        public MainPageViewModel(WorkScheduleRepositoryDB scheduleRepo, WorkShiftRepositoryDB shiftsRepo)
        {
            this.scheduleRepo = scheduleRepo;
            this.shiftsRepo = shiftsRepo;

            this.schedule = new Schedule();
            this.scheduleNames = new ObservableCollection<string>();
            this.calendarDays = new ObservableCollection<CalendarDay>();

            this.dbShifts = new List<DbShift>();

            //_ = RefreshThePage();
        }

        public async Task RefreshThePage()
        {
            calendarDays.Clear();
            this.schedule.WorkSchedule.Clear(); // Clear the current schedule

            await LoadLastSchedule(); //Load the last schedule
            await LoadScheduleNamesAsync(); //Load schedules names
            //Initialize the calendar view grid
            if (this.schedule.WorkSchedule.Count > 0)
                await GenerateCalendar(schedule.WorkSchedule[0].Year, schedule.WorkSchedule[0].Month);
            else
                await GenerateBlankCalendar();

        }

        private async Task LoadScheduleNamesAsync()
        {

            ScheduleNames.Clear();
            ScheduleNames = new ObservableCollection<string>(await scheduleRepo.GetAllScheduleNames());

            //var schedulesNameOnly = await scheduleRepo.GetAllScheduleNames();
            //if(schedulesNameOnly.Count>0)
            //{
            //    foreach (var names in schedulesNameOnly)
            //        ScheduleNames.Add(names);
            //}
        }

        private async Task LoadLastSchedule()
        {
            // Fetch the most recent schedule from the database
            this.dbSchedule = await scheduleRepo.GetLastSchedule();
            await LoadFromDBToSchedule();
        }
        private async Task LoadFromDBToSchedule()
        {
            if (dbSchedule != null)
            {
                dbShifts = await shiftsRepo!.GetShiftsForSchedule(dbSchedule.Id);
            }

            if (dbShifts != null)
            {
                foreach (var dbShift in dbShifts)
                {
                    IShift? shift = null;

                    if (dbShift.ShiftType == ShiftType.DayShift)
                    {
                        shift = new DayShift(ShiftType.DayShift, dbShift.Year, dbShift.Month, dbShift.Day,
                            dbShift.StarTime, dbShift.ShiftHour, dbShift.IsCompensated);
                    }
                    else if (dbShift.ShiftType == ShiftType.NightShift)
                    {

                        shift = new NightShift(ShiftType.NightShift, dbShift.Year, dbShift.Month, dbShift.Day,
                            dbShift.StarTime, dbShift.ShiftHour, dbShift.IsCompensated);
                    }
                    else
                    {
                        shift = new RestDay(ShiftType.RestDay, dbShift.Year, dbShift.Month, dbShift.Day,
                                                dbShift.StarTime, dbShift.ShiftHour, dbShift.IsCompensated);
                    }

                    await this.schedule.AddShift(shift!);
                }
            }
        }

        public Task GenerateCalendar(int year, int month)
        {
            calendarDays.Clear(); //Maybe not necessary 

            CalendarMonthYear = $"Work Schedule for {Provider.GetMonthName(month)} {year} ";

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            GeneratePreviousMonthDays(calendarDays, firstDayOfMonth);

            foreach (var shift in schedule.WorkSchedule)
            {
                string color = "White"; // Default color for rest days
                string shiftInicial= "R"; // Default for rest days  
                if (shift.IsCompensated)
                {
                    color = "LightBlue";
                    shiftInicial= "C"; // Compensated
                }
                else if (shift.ShiftType == ShiftType.DayShift)
                {
                    color = "LightGreen";
                    shiftInicial= "D";

                }
                else if (shift.ShiftType == ShiftType.NightShift)
                {
                    color = "LightCoral";
                    shiftInicial = "N";
                }

                calendarDays.Add(new CalendarDay
                {
                    Day = $"{shift.Day} {shiftInicial}",
                    BackgroundColor = color,
                    IsCurrentMonth = true
                });
            }

            GenerateNextMonthDays(calendarDays);

            return Task.CompletedTask;
        }

        public async Task GenerateBlankCalendar()
        {
            calendarDays.Clear(); //Maybe not necessary 

            int year= DateTime.Now.Year;
            int month = DateTime.Now.Month;

            CalendarMonthYear = $"The month is {month} / {year} ";

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);


            await GeneratePreviousMonthDays(calendarDays, firstDayOfMonth);

            for (int day = 1; day <= daysInMonth; day++)
            {
                string color = "White";

                calendarDays.Add(new CalendarDay
                {
                    Day = $"{day}",
                    BackgroundColor = color
                });
            }


            await GenerateNextMonthDays(calendarDays);

        }


        private Task GeneratePreviousMonthDays(ObservableCollection<CalendarDay> calendarDays, DateTime firstDayOfMonth)
        {
            int offsetStart = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            var prevMonth = firstDayOfMonth.AddMonths(-1);
            int prevMonthDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);

            for (int i = offsetStart - 1; i >= 0; i--)
            {
                calendarDays.Add(new CalendarDay
                {
                    Day = (prevMonthDays - i).ToString(),
                    BackgroundColor = "LightGray",
                    IsCurrentMonth = false
                });
            }
            return Task.CompletedTask;
        }


        private Task GenerateNextMonthDays(ObservableCollection<CalendarDay> calendarDays)
        {
            int totalDays = calendarDays.Count;
            int paddingEnd = 42 - totalDays;

            for (int i = 1; i <= paddingEnd; i++)
            {
                calendarDays.Add(new CalendarDay
                {
                    Day = i.ToString(),
                    BackgroundColor = "LightGray",
                    IsCurrentMonth = false
                });
            }
            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task LoadSavedSchedule()
        {
            calendarDays.Clear();

            dbSchedule = await this.scheduleRepo.GetScheduleByName(SelectedScheduleName);
            
            await LoadFromDBToSchedule();

            await RefreshThePage();
        }


        [RelayCommand]
        private async Task PickNewSchedule()
        {
            await Shell.Current.GoToAsync(nameof(PickerDatePage));
        }

        //[RelayCommand]
        //private void ExitApp()
        //{
        //    Application.Current!.Quit();
        //}
    }
}
