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
            if (this.Schedule.WorkSchedule.Count == 0 || this.Schedule == null)
            {
                await GenerateBlankCalendar();
            }
            else
            {
                await GenerateCalendarMonthYearAndTotalHours();
            }
        }

        private async Task LoadLastSchedule()
        {
            try
            {
                this.dbSchedule = await scheduleRepo.GetLastSchedule();
                if (this.dbSchedule != null)
                {
                    await LoadFromDBToSchedule();
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in LoadLastSchedule in the MainViewMode.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        private async Task LoadFromDBToSchedule()
        {
            this.Schedule.WorkSchedule.Clear();

            try
            {
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
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in LoadFromDBToSchedule in the MainViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

            if (this.Schedule.WorkSchedule.Count == 0 || this.Schedule == null)
            {
                await GenerateBlankCalendar();
            }
            else
            {
                await GenerateCalendarMonthYearAndTotalHours();
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
                await ShowPopupMessage(AppResources.Information, AppResources.ThisShiftIsNotInTheCurrentMonth);
                this.SelectedShift = null;
                return;
            }

            if (this.SelectedShift.ShiftType == ShiftType.RestDay)
            {
                await ShowPopupMessage(AppResources.Information, AppResources.ThisIsARestDay);
                this.SelectedShift = null;
                return;
            }

            try
            {
                var popup = new ShiftInfoPopup(this.SelectedShift);
                await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ShiftInfoPopup in the MainViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

            this.SelectedShift = null;
            return;
        }

        private async Task GenerateBlankCalendar()
        {
            await UpdateCalendarMonthYear(DateTime.Now.Month, DateTime.Now.Year);

            try
            {
                IEngine<ISchedule<IShift>> engine = new Engine();
                var tempSchedule = await engine.BlankCalendar();

                foreach (var shift in tempSchedule.WorkSchedule)
                {
                    await this.Schedule.AddShift(shift);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in BlankCalendar method in the Engine (Structure) class");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }


            this.TextMessage = String.Format(AppResources.TextMessageThisIsACalendar,
                                            CulturalProvider.GetMonthName(DateTime.Now.Month), DateTime.Now.Year);
        }

        private async Task GenerateCalendarMonthYearAndTotalHours()
        {
            DateTime startDate = this.Schedule.WorkSchedule
                                                       .Where(s => s.IsCurrentMonth == true)
                                                       .First()
                                                       .GetDateShift();

            int year = startDate.Year;
            int month = startDate.Month;

            await UpdateCalendarMonthYear(month, year);

            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            if(monthByHoursTotal.Key == 0 || monthByHoursTotal.Value.Length==0)
            {

               await ShowPopupMessage(AppResources.Error, AppResources.AtThisMomentWorkHoursAvailableTo2025);
                this.TextMessage = "";
                return;
            }

            string monthName = CulturalProvider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);
            int totalHours = await this.Schedule.CalculateTotalWorkHours();

            this.TextMessage = String
                                .Format(AppResources.TextMessageTotalHoursAreForTheMonthAndWorkingHours,
                                        monthName, totalHours, totalHoursByMonth);
        }

        private async Task UpdateCalendarMonthYear(int month, int year)
        {
            await Task.Delay(10);
            this.CalendarMonthYear = String.Format(AppResources.CalendarWorkScheduleFor, CulturalProvider.GetMonthName(month), year);
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }

        [RelayCommand]
        private async Task GoToEditSchedule()
        {
            await Shell.Current.GoToAsync(nameof(SchedulePage));
        }
    }
}
