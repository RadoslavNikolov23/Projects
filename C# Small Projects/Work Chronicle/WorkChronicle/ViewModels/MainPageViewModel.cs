namespace WorkChronicle.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        private IShift? selectedShift;

        public IRelayCommand<SelectionChangedEventArgs> ShiftSelectedCommand { get; }



        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB? shiftsRepo;


        private DbSchedule? dbSchedule;

        private IList<DbShift>? dbShifts;

        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<string> scheduleNames;

        [ObservableProperty]
        private string selectedScheduleName = "";

        [ObservableProperty]
        private string calendarMonthYear = "";

        [ObservableProperty]
        private string textMessage = "";


        public MainPageViewModel(ISchedule<IShift> schedule, WorkScheduleRepositoryDB scheduleRepo, WorkShiftRepositoryDB shiftsRepo)
        {
            this.ShiftSelectedCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(OnShiftSelected!);

            this.scheduleRepo = scheduleRepo;
            this.shiftsRepo = shiftsRepo;

            this.schedule = schedule;
            this.scheduleNames = new ObservableCollection<string>();

            this.dbShifts = new List<DbShift>();
        }

        private async Task OnShiftSelected(SelectionChangedEventArgs args)
        {
            // Check if the selected item is null or not
            if(this.SelectedShift==null)
                return;

            if (this.SelectedShift.ShiftType == ShiftType.RestDay)
            {
                await Shell.Current.DisplayAlert("Information", "This is a Rest Day .", "OK");
                // Deselect the item
                this.SelectedShift = null;
                return;
            }

            // Display a information about the selected shift
            await Shell.Current.DisplayAlert("Information", 
                                            $"""
                                            This is a {this.SelectedShift.ShiftType.ToString()}.
                                            Your start the shift at {this.SelectedShift.StarTime} and
                                            is {this.SelectedShift.ShiftHour} hours long.
                                            """, "OK");
            // Deselect the item
            this.SelectedShift = null;
            return;



            //var popup = new ShiftCompensationPopup(this.SelectedShift!);
            // await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            //-TO DELTE !!
            //if (args.CurrentSelection.FirstOrDefault() is IShift selected)
            //{
            //    if (!selected.IsCurrentMonth)
            //    {
            //        // Deselect the item
            //        this.SelectedShift = null;
            //        return;
            //    }


            //    var popup = new ShiftCompensationPopup(this.SelectedShift!);
            //    await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            //    // Do whatever logic you want with the valid selection
            //    this.SelectedShift = selected;
            //}
        }


        public async Task RefreshThePage()
        {
            await LoadScheduleNamesAsync();

            await LoadLastSchedule();

            if (this.Schedule.WorkSchedule.Count == 0)
            {
                //await GenerateBlankCalendar(); --- TO MAKE!!!!!!
                TextMessage = "You have not generated any schedule yet!.";
            }
            else
            {
                DateTime startDate = Schedule.WorkSchedule
                                       .Where(s => s.IsCurrentMonth == true)
                                       .First()
                                       .GetDateShift();

                int year = startDate.Year;
                int month = startDate.Month;
                CalendarMonthYear = $"Work Schedule for {Provider.GetMonthName(month)} {year} ";

                KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

                string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
                int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);
                int totalHours = await this.Schedule.CalculateTotalWorkHours();

                TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";
            }
        }

        private async Task LoadScheduleNamesAsync()
        {
            ScheduleNames.Clear();

            List<string> scheduleNamesDTO = await scheduleRepo.GetAllScheduleNames();

            if (scheduleNamesDTO.Count > 0)
            {
                foreach (var names in scheduleNamesDTO)
                    ScheduleNames.Add(names);
            }
            else
            {
                ScheduleNames.Add("No schedules found");
            }
        }

        private async Task LoadLastSchedule()
        {
            this.dbSchedule = await scheduleRepo.GetLastSchedule();
            if (dbSchedule != null)
            {
                await LoadFromDBToSchedule();
            }
        }
        private async Task LoadFromDBToSchedule()
        {
            this.Schedule.WorkSchedule.Clear(); // Clear the current schedule

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
            }
        }

        //---------Make this method to generate a blank calendar for the current month---------
        /*
         public async Task GenerateBlankCalendar()
         {
            // calendarDays.Clear(); //Maybe not necessary 

             int year = DateTime.Now.Year;
             int month = DateTime.Now.Month;

             CalendarMonthYear = $"The month is {month} / {year} ";

             DateTime firstDayOfMonth = new DateTime(year, month, 1);
             int daysInMonth = DateTime.DaysInMonth(year, month);


            // await GeneratePreviousMonthDays(calendarDays, firstDayOfMonth);

             for (int day = 1; day <= daysInMonth; day++)
             {
                 string color = "White";

                 //calendarDays.Add(new CalendarDay
                 //{
                 //    Day = $"{day}",
                 //    BackgroundColor = color
                 //});
             }


            // await GenerateNextMonthDays(calendarDays);

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
        */

        [RelayCommand]
        private async Task LoadSavedSchedule()
        {
            dbSchedule = await this.scheduleRepo
                                    .GetScheduleByName(SelectedScheduleName);

            await LoadFromDBToSchedule();

            // await RefreshThePage();

            //Chech if the schedule is loaded correctly without the Refresh Page and make
            // this into a method
            int year = this.Schedule.WorkSchedule
                                    .Where(s => s.IsCurrentMonth)
                                    .First()
                                    .Year;

            int month = this.Schedule.WorkSchedule
                                     .Where(s => s.IsCurrentMonth)
                                     .First()
                                     .Month;

            CalendarMonthYear = $"Work Schedule for {Provider.GetMonthName(month)} {year} ";
        }


        [RelayCommand]
        private async Task PickNewSchedule()
        {
            await Shell.Current.GoToAsync(nameof(PickerDatePage));
        }
    }
}
