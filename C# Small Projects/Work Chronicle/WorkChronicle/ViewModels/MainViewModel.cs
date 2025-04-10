namespace WorkChronicle.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB shiftsRepo;

        private DbSchedule? dbSchedule;

        private List<DbShift>? dbShifts;

        [ObservableProperty]
        private ISchedule<IShift> schedule;

        //[ObservableProperty]
        //private ObservableCollection<IShift> workSchedule;

        [ObservableProperty]
        private IShift? selectedShift;

        [ObservableProperty]
        private string calendarMonthYear = "";

        [ObservableProperty]
        private string textMessage = "";

        public IRelayCommand<SelectionChangedEventArgs> ShiftSelectedCommand { get; }

        public MainViewModel(ISchedule<IShift> schedule, WorkScheduleRepositoryDB scheduleRepo, WorkShiftRepositoryDB shiftsRepo)
        {   
            this.schedule = schedule;

            this.scheduleRepo = scheduleRepo;
            this.shiftsRepo = shiftsRepo;

            this.dbSchedule = new DbSchedule();
            this.dbShifts = new List<DbShift>();

            this.ShiftSelectedCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(OnShiftSelected!);
            
            _ = LoadLastSchedule();
        }

        public async Task RefreshThePage()
        {
            // await LoadLastSchedule(); //- CHeck if this load only when the app is started

            if (this.Schedule.WorkSchedule.Count == 0 || this.Schedule==null)
            {
                await GenerateBlankCalendar();
            }
            else
            {
                DateTime startDate = Schedule.WorkSchedule
                                       .Where(s => s.IsCurrentMonth == true)
                                       .First()
                                       .GetDateShift();

                int year = startDate.Year;
                int month = startDate.Month;
                await UpdateCalendarMonthYear(month, year);
                await GenerateTotalHours(startDate);
            }
        }

        private async Task LoadLastSchedule()
        {
            this.dbSchedule = await scheduleRepo.GetLastSchedule();
            if (this.dbSchedule != null)
            {
                await LoadFromDBToSchedule();
            }
        }
        private async Task LoadFromDBToSchedule()
        {
            this.Schedule.WorkSchedule.Clear();

            if (this.dbSchedule != null)
            {
                this.dbShifts = await shiftsRepo!.GetShiftsForSchedule(this.dbSchedule.Id);
            }

            if (this.dbShifts != null && this.dbShifts.Count > 0)
            {
                foreach (var dbShift in this.dbShifts)
                {
                    IShift? shift = null;

                    if (dbShift.ShiftType == ShiftType.DayShift)
                    {
                        shift = new DayShift(ShiftType.DayShift,
                                            dbShift.Year,
                                            dbShift.Month,
                                            dbShift.Day,
                                            dbShift.StarTime,
                                            dbShift.ShiftHour,
                                            dbShift.IsCurrentMonth,
                                            dbShift.IsCompensated);
                    }
                    else if (dbShift.ShiftType == ShiftType.NightShift)
                    {
                        shift = new NightShift(ShiftType.NightShift,
                                                dbShift.Year,
                                                dbShift.Month,
                                                dbShift.Day,
                                                dbShift.StarTime,
                                                dbShift.ShiftHour,
                                                dbShift.IsCurrentMonth,
                                                dbShift.IsCompensated);
                    }
                    else
                    {
                        shift = new RestDay(ShiftType.RestDay,
                                            dbShift.Year,
                                            dbShift.Month,
                                            dbShift.Day,
                                            dbShift.StarTime,
                                            dbShift.ShiftHour,
                                            dbShift.IsCurrentMonth,
                                            dbShift.IsCompensated);
                    }

                    await this.Schedule.AddShift(shift!);
                }

                if (this.Schedule.WorkSchedule.Count == 0 || this.Schedule == null)
                {
                    await GenerateBlankCalendar();
                }
                else
                {
                   DateTime startDate = Schedule.WorkSchedule
                     .Where(s => s.IsCurrentMonth == true)
                     .First()
                     .GetDateShift();

                    int year = startDate.Year;
                    int month = startDate.Month;
                    await UpdateCalendarMonthYear(month, year);
                    await GenerateTotalHours(startDate);
                }
            }
        }

        private async Task OnShiftSelected(SelectionChangedEventArgs args)
        {
            if (this.SelectedShift == null)
            {
                return;
            }

            if (this.SelectedShift.IsCurrentMonth == false)
            {
                await ShowPopupMessage("Info", "This shift is not in the current month.");
                this.SelectedShift = null;
                return;
            }

            if (this.SelectedShift.ShiftType == ShiftType.RestDay)
            {
                await ShowPopupMessage("Information", "This is a Rest Day.");
                this.SelectedShift = null;
                return;
            }

            var popup = new ShiftInfoPopup(this.SelectedShift);
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            this.SelectedShift = null;
            return;
            //FOR DELETING-----
            //await Shell.Current.DisplayAlert("Information",
            //                                $"""
            //                                This is a {this.SelectedShift.ShiftType.ToString()}.
            //                                Your start the shift at {this.SelectedShift.StarTime} and
            //                                is {this.SelectedShift.ShiftHour} hours long.
            //                                """, "OK");

        }

        private async Task UpdateCalendarMonthYear(int month, int year)
        {
            await Task.Delay(50);   // Just to simulate some delay
            this.CalendarMonthYear = $"Work Schedule for {Provider.GetMonthName(month)} {year} ";
        }

        private async Task GenerateTotalHours(DateTime startDate)
        {
            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);
            int totalHours = await this.Schedule.CalculateTotalWorkHours();

            this.TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";
        }

        private async Task GenerateBlankCalendar()
        {
            await UpdateCalendarMonthYear(DateTime.Now.Month, DateTime.Now.Year);

            IEngine<ISchedule<IShift>> engine = new Engine();
            this.Schedule = await engine.BlankCalendar();

            this.TextMessage = $"This is a calendar for the {Provider.GetMonthName(DateTime.Now.Month)} {DateTime.Now.Year}!";
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
