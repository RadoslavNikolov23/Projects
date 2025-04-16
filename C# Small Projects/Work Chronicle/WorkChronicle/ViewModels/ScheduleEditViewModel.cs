namespace WorkChronicle.ViewModels
{
    public partial class ScheduleEditViewModel : BaseViewModel
    {
        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB shiftRepo;

        [ObservableProperty]
        private ISchedule<IShift> schedule;

        public IRelayCommand<SelectionChangedEventArgs> ShiftSelectedCommand { get; }

        [ObservableProperty]
        private IShift? selectedShift;

        [ObservableProperty]
        private string textMessage = "";

        [ObservableProperty]
        private string hoursMessage = "";

        [ObservableProperty]
        private string calendarMonthYear = "";

        DateTime startDate;

        public ScheduleEditViewModel(ISchedule<IShift> schedule, WorkShiftRepositoryDB shiftRepositoryDB, WorkScheduleRepositoryDB scheduleRepositoryDB)
        {
            this.schedule = schedule;
            this.scheduleRepo = scheduleRepositoryDB;
            this.shiftRepo = shiftRepositoryDB;

            this.ShiftSelectedCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(OnShiftSelected!);
            this.startDate = new DateTime();
        }
        public async Task RefreshThePage()
        {
            if (this.Schedule.WorkSchedule.Count == 0 || this.Schedule.WorkSchedule == null)
            {
                this.TextMessage = AppResources.YouHaveNoScheduleForThisMonth;
            }
            else
            {
                this.startDate = this.Schedule.WorkSchedule
                                                  .Where(s => s.IsCurrentMonth == true)
                                                  .First()
                                                  .GetDateShift();

                await UpdateCalendarMonthYear();
                await GenerateTotalHours();
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
                await ShowPopupMessage(AppResources.Information, 
                                       AppResources.ThisShiftIsNotInTheCurrentMonth);
                this.SelectedShift = null;
                return;
            }
        }

        [RelayCommand]
        private async Task CompensateButton()
        {
            if (this.SelectedShift == null)
                return;

            if (this.SelectedShift.ShiftType == ShiftType.RestDay)
            {
                await ShowPopupMessage(AppResources.Information, 
                                       AppResources.ThisIsARestDayAndItCantBeCompensated);
                this.SelectedShift = null;
                return;
            }

            try
            {
                DbSchedule tempDBSchedule = await this.scheduleRepo.GetScheduleByName(
                               $"{Provider.GetMonthName(this.SelectedShift.Month)} {this.SelectedShift.Year}");

                IShift? shift = this.Schedule.WorkSchedule
                                                   .Where(s => s.Year == this.SelectedShift.Year
                                                               && s.Month == this.SelectedShift.Month
                                                               && s.Day == this.SelectedShift.Day)
                                                   .FirstOrDefault();
                if (shift == null)
                {
                    await ShowPopupMessage(AppResources.Error,
                                            AppResources.SomethingWentWrongPleaseTryAgain);
                    this.SelectedShift = null;
                    return;

                }

                this.SelectedShift.IsCompensated = !this.SelectedShift.IsCompensated;
                shift.IsCompensated = this.SelectedShift.IsCompensated;

                if (tempDBSchedule != null)
                {
                    DbShift dbShift = await this.shiftRepo.GetSingleShifts(tempDBSchedule.Id,
                                                                           this.SelectedShift.Year,
                                                                           this.SelectedShift.Month,
                                                                           this.SelectedShift.Day);

                    if (dbShift == null)
                    {
                        await ShowPopupMessage(AppResources.Error,
                                               AppResources.SomethingWentWrongPleaseTryAgain);
                        shift!.IsCompensated = !this.SelectedShift.IsCompensated;
                        return;
                    }

                    dbShift.IsCompensated = this.SelectedShift.IsCompensated;
                    int numberRowsAffected = await this.shiftRepo.UpdateShift(dbShift);

                    if (numberRowsAffected == 0)
                    {
                        await ShowPopupMessage(AppResources.Error,
                                               AppResources.SomethingWentWrongPleaseTryAgain);
                        dbShift.IsCompensated = !dbShift.IsCompensated;
                        shift!.IsCompensated = !this.SelectedShift.IsCompensated;
                        this.SelectedShift = null;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in CompensateButton in the ScheduleEditViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

            if (this.SelectedShift!.IsCompensated)
            {
                await ShowPopupMessage(AppResources.Information, 
                                       AppResources.SelectedShiftIsCompensated);
            }
            else
            {
                await ShowPopupMessage(AppResources.Information,
                                       AppResources.SelectedShiftIsUnCompensated);
            }

            this.SelectedShift = null;
            await GenerateTotalHours();
            return;
        }

        [RelayCommand]
        private async Task EditShift()
        {
            if (this.SelectedShift == null)
                return;

            try
            {
                var popup = new ShiftEditPopup(this.SelectedShift!);
                var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

       
                if (result is IShift editedShift) //TODO make an alternative version
                {
                    this.SelectedShift = editedShift;
                }
                else
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    this.SelectedShift = null;
                    return;
                }

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ShiftEditPopup in the ScheduleEditViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

            try
            {
                DbSchedule tempDBSchedule = await this.scheduleRepo.GetScheduleByName(
                        $"{Provider.GetMonthName(this.SelectedShift!.Month)} {this.SelectedShift.Year}");

                IShift? shift = this.Schedule.WorkSchedule
                                                  .Where(s => s.Year == this.SelectedShift.Year
                                                              && s.Month == this.SelectedShift.Month
                                                              && s.Day == this.SelectedShift.Day)
                                                  .FirstOrDefault();
                if (shift == null)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    this.SelectedShift = null;
                    return;
                }

                ShiftType originalShiftType = shift.ShiftType;
                double originalStarTime = shift.StarTime;
                double originalShiftHour = shift.ShiftHour;
                bool originalIsCompensated = shift.IsCompensated;

                shift.ShiftType = this.SelectedShift.ShiftType;
                shift.StarTime = this.SelectedShift.StarTime;
                shift.ShiftHour = this.SelectedShift.ShiftHour;
                shift.IsCompensated = this.SelectedShift.IsCompensated;

                if (tempDBSchedule != null)
                {
                    DbShift dbShift = await this.shiftRepo.GetSingleShifts(tempDBSchedule.Id,
                                                                           this.SelectedShift.Year,
                                                                           this.SelectedShift.Month,
                                                                           this.SelectedShift.Day);

                    if (dbShift == null)
                    {
                        await ShowPopupMessage(AppResources.Error,
                                               AppResources.SomethingWentWrongPleaseTryAgain);

                        shift.ShiftType = originalShiftType;
                        shift.StarTime = originalStarTime;
                        shift.ShiftHour = originalShiftHour;
                        shift.IsCompensated = originalIsCompensated;

                        return;
                    }

                    ShiftType dbOriginalShiftType = dbShift.ShiftType;
                    double dbOriginalStarTime = dbShift.StarTime;
                    double dbOriginalShiftHour = dbShift.ShiftHour;

                    dbShift.ShiftType = this.SelectedShift.ShiftType;
                    dbShift.StarTime = this.SelectedShift.StarTime;
                    dbShift.ShiftHour = this.SelectedShift.ShiftHour;
                    dbShift.IsCompensated = false;

                    int numberRowsAffected = await this.shiftRepo.UpdateShift(dbShift);

                    if (numberRowsAffected == 0)
                    {
                        await ShowPopupMessage(AppResources.Error,
                                               AppResources.SomethingWentWrongPleaseTryAgain);

                        shift.ShiftType = originalShiftType;
                        shift.StarTime = originalStarTime;
                        shift.ShiftHour = originalShiftHour;
                        shift.IsCompensated = originalIsCompensated;

                        dbShift.ShiftType = dbOriginalShiftType;
                        dbShift.StarTime = dbOriginalStarTime;
                        dbShift.ShiftHour = dbOriginalShiftHour;
                        dbShift.IsCompensated = originalIsCompensated;

                        this.SelectedShift = null;
                        return;
                    }
                }
                else
                {
                    await ShowPopupMessage(AppResources.Information, AppResources.YourShiftHasBeenEditSaveItFirst);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in EditShift in the ScheduleEditViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

            await ShowPopupMessage(AppResources.Information, AppResources.YourShiftHasBeenEdit);

            await GenerateTotalHours();
            this.SelectedShift = null;
            return;
        }

        [RelayCommand]
        private async Task SaveShiftSchedule()
        {
            int resultNumber = 0;

            int year = this.Schedule.WorkSchedule
                             .Where(s => s.IsCurrentMonth)
                             .First()
                             .Year;

            int month = this.Schedule.WorkSchedule
                                     .Where(s => s.IsCurrentMonth)
                                     .First()
                                     .Month;

            try
            {
                DbSchedule dbSchedule = new DbSchedule
                {
                    ScheduleName = $"{Provider.GetMonthName(month)} {year}",
                    Year = year,
                    Month = month
                };

                if (dbSchedule == null)
                {
                    await ShowPopupMessage(AppResources.Error,
                                           AppResources.SomethingWentWrongGeneratedNewSchedule);
                    return;
                }

                DbSchedule existingDbSchedule = await this.scheduleRepo
                                                        .GetScheduleByName(dbSchedule.ScheduleName);

                if (existingDbSchedule != null)
                {
                    bool overwrite = await Shell.Current
                                                .DisplayAlert(
                                                    AppResources.ScheduleExists,
                                                    String.Format(AppResources.AScheduleForTheMonthAlreadyExistsToOverwrite, dbSchedule.ScheduleName),
                                                    "OK", // Confirm overwrite
                                                    "Cancel" // Cancel
                                                    );
                    if (!overwrite)
                        return;

                    resultNumber = await this.scheduleRepo.DeleteSchedule(existingDbSchedule);

                    if (resultNumber == 0)
                    {
                        await ShowPopupMessage(AppResources.Error,
                                               AppResources.SomethingWentWrongPleaseTryAgain);
                        return;
                    }
                }

                resultNumber = await this.scheduleRepo.AddSchedule(dbSchedule);

                if (resultNumber == 0)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    return;
                }

                resultNumber = 0;
                foreach (var shifts in Schedule.WorkSchedule)
                {
                    DbShift dbShift = new DbShift
                    {
                        ShiftType = shifts.ShiftType,
                        Year = shifts.Year,
                        Month = shifts.Month,
                        Day = shifts.Day,
                        StarTime = shifts.StarTime,
                        ShiftHour = shifts.ShiftHour,
                        IsCurrentMonth = shifts.IsCurrentMonth,
                        IsCompensated = shifts.IsCompensated,
                        DbScheduleId = dbSchedule.Id
                    };

                    resultNumber += await this.shiftRepo.AddShift(dbShift);
                }

                if (resultNumber == Schedule.WorkSchedule.Count)
                {
                    await ShowPopupMessage(AppResources.Information, AppResources.YourScheduleHasBeenSaved);
                }
                else
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
                    return;
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in SaveShiftSchedule in the ScheduleEditViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }
        }

        [RelayCommand]
        private async Task GoBackButton()
        {
            await Shell.Current.GoToAsync("//PickerDatePage");
            await Task.Delay(2);
            await Shell.Current.GoToAsync("///MainPage");
        }

        private async Task GenerateTotalHours()
        {
            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(this.startDate);

            if (monthByHoursTotal.Key == 0 || monthByHoursTotal.Value.Length == 0)
            {
                await ShowPopupMessage(AppResources.Error, AppResources.AtThisMomentWorkHoursAvailableTo2025);
                this.TextMessage = "";
                return;
            }

            string monthName = CulturalProvider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);
            int totalHours = await this.Schedule.CalculateTotalWorkHours();

            this.TextMessage = String.Format            
                                    (AppResources.TextMessageTotalHoursAreForTheMonthAndWorkingHours,
                                      monthName, totalHours, totalHoursByMonth);

            if (totalHours > totalHoursByMonth)
            {
                this.HoursMessage = String.Format(AppResources.HourMessageYouHaveHoursOfOvertime, totalHours - totalHoursByMonth);
            }
            else
            {
                this.HoursMessage = String.Format
                                        (AppResources.HourMessageYouHaveUnderTheTotalHoursForTheMonth, 
                                        totalHoursByMonth - totalHours);
            }
        }

        private async Task UpdateCalendarMonthYear()
        {
            await Task.Delay(10);

            int year = this.startDate.Year;
            int month = this.startDate.Month;

            this.CalendarMonthYear = String.Format(AppResources.CalendarWorkScheduleFor, CulturalProvider.GetMonthName(month), year);
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
