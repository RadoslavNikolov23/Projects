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
        private string? selectedScheduleName = "";

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
            await LoadScheduleNames();
        }

        private async Task LoadScheduleNames()
        {
            this.ScheduleNames.Clear();

            try
            {
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
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in LoadScheduleNames in the LoadScheduleViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        [RelayCommand]
        private async Task DeleteSelectedSchedule()
        {
            if (string.IsNullOrEmpty(this.SelectedScheduleName))
            {
                await ShowPopupMessage(AppResources.Error, AppResources.PleaseSelectASchedule);
                this.SelectedScheduleName = null;
                return;
            }

            try
            {
                this.dbSchedule = await this.scheduleRepo
                                                    .GetScheduleByName(this.SelectedScheduleName);

                if (this.dbSchedule == null)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.ScheduleNotFoundPleaseTryAgain);
                    this.SelectedScheduleName = null;
                    return;
                }

                int resultNumber = await this.scheduleRepo.DeleteSchedule(this.dbSchedule);

                if (resultNumber == 0)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    return;
                }

                await ShowPopupMessage(AppResources.Information, AppResources.ScheduleDeletedSuccessfully);
                this.SelectedScheduleName = null;
                await LoadScheduleNames();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in DeleteSelectedSchedule in the LoadScheduleViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        [RelayCommand]
        private async Task LoadSelectedSchedule()
        {
            if (string.IsNullOrEmpty(this.SelectedScheduleName))
            {
                await ShowPopupMessage(AppResources.Error, AppResources.PleaseSelectASchedule);
                this.SelectedScheduleName = null;
                return;
            }

            try
            {
                this.dbSchedule = await this.scheduleRepo
                                   .GetScheduleByName(this.SelectedScheduleName);

                if (dbSchedule == null)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    this.SelectedScheduleName = null;
                    return;
                }

                this.SelectedScheduleName = null;

                await LoadFromDBToSchedule();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in LoadSelectedSchedule in the LoadScheduleViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        private async Task LoadFromDBToSchedule()
        {
            this.Schedule.WorkSchedule.Clear();

            try
            {
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
                        await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    }
                    else
                    {
                        DateTime startDate = Schedule.WorkSchedule
                                                  .Where(s => s.IsCurrentMonth == true)
                                                  .First()
                                                  .GetDateShift();

                        int year = startDate.Year;
                        int month = startDate.Month;

                        await ShowPopupMessage(AppResources.Information, String.Format(AppResources.YouHaveLoadedTheScheduleForThe, Provider.GetMonthName(month), year));

                        await Shell.Current.GoToAsync("///MainPage");
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in LoadFromDBToSchedule in the LoadScheduleViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
