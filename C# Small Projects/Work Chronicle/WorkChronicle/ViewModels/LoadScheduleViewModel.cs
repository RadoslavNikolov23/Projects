namespace WorkChronicle.ViewModels
{
    public partial class LoadScheduleViewModel : BaseViewModel
    {
        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB? shiftsRepo;

        private DbSchedule? dbSchedule;

        private List<DbShift>? dbShifts;

        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<string> scheduleNames;

        [ObservableProperty]
        private string? selectedScheduleName="";

        [ObservableProperty]
        private bool isLabelEmptyVisible = false;

        public LoadScheduleViewModel(ISchedule<IShift> schedule, WorkScheduleRepositoryDB scheduleRepo, WorkShiftRepositoryDB shiftsRepo)
        {
            this.schedule = schedule;
            this.scheduleRepo = scheduleRepo;
            this.shiftsRepo = shiftsRepo;

            this.dbSchedule = new DbSchedule();
            this.dbShifts = new List<DbShift>();

            this.scheduleNames = new ObservableCollection<string>();
        }

        public async Task RefreshThePage()
        {
            await LoadScheduleNamesAsync();
        }

        private async Task LoadScheduleNamesAsync()
        {
            this.ScheduleNames.Clear();

            IList<string> dbScheduleNames = await scheduleRepo.GetAllScheduleNames();

            if (dbScheduleNames.Count > 0)
            {
                this.IsLabelEmptyVisible = false;

                foreach (var dbScheduleName in dbScheduleNames)
                    this.ScheduleNames.Add(dbScheduleName);
            }
            else
            {
                this.IsLabelEmptyVisible = true;
            }
        }

        [RelayCommand]
        private async Task LoadSelectedSchedule()
        {
            if (string.IsNullOrEmpty(this.SelectedScheduleName))
            {
                await ShowPopupMessage("Error", "Please select a schedule.");
                this.SelectedScheduleName = null;
                return;
            }

            this.dbSchedule = await this.scheduleRepo
                                    .GetScheduleByName(this.SelectedScheduleName);

            if (dbSchedule == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, try again");
                this.SelectedScheduleName = null;
                return;
            }

            await LoadFromDBToSchedule();

            this.SelectedScheduleName = null;
        }

        private async Task LoadFromDBToSchedule()
        {
            this.Schedule.WorkSchedule.Clear();

            this.dbShifts = await shiftsRepo!.GetShiftsForSchedule(dbSchedule!.Id);

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
                    await ShowPopupMessage("Error", "Something went wrong, try again");
                }
                else
                {
                    DateTime startDate = Schedule.WorkSchedule
                                              .Where(s => s.IsCurrentMonth == true)
                                              .First()
                                              .GetDateShift();

                    int year = startDate.Year;
                    int month = startDate.Month;

                    await ShowPopupMessage("Success", $"You have loaded the {Provider.GetMonthName(month)} schedule for the {year}, go to the Home page to check the schedule");

                    //await Shell.Current.GoToAsync("///MainPage");
                }
            }
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
