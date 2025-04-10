namespace WorkChronicle.ViewModels
{ 
    public partial class ScheduleEditViewModel : BaseViewModel
    {
        private WorkScheduleRepositoryDB scheduleRepo;

        private WorkShiftRepositoryDB shiftRepo;

        private DbSchedule? dbSchedule;

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

        public ScheduleEditViewModel(ISchedule<IShift> schedule, WorkShiftRepositoryDB shiftRepositoryDB, WorkScheduleRepositoryDB scheduleRepositoryDB)
        {
            this.schedule = schedule;
            this.scheduleRepo = scheduleRepositoryDB;
            this.shiftRepo = shiftRepositoryDB;

            this.dbSchedule = new DbSchedule();
            this.ShiftSelectedCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(OnShiftSelected!);
        }

        private async Task OnShiftSelected(SelectionChangedEventArgs args)
        {
            await Task.Delay(10); // Simulate a delay for the popup to show

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
        }

        public async Task RefreshThePage()
        {
            if (Schedule.WorkSchedule.Count == 0 || this.Schedule.WorkSchedule == null)
            {
                this.TextMessage = "You have no shifts for this month.";
            }
            else
            {
                DateTime startDateNew = Schedule.WorkSchedule
                                            .Where(s => s.IsCurrentMonth == true)
                                            .First()
                                            .GetDateShift();

                int year = startDateNew.Year;
                int month = startDateNew.Month;

                await GenerateShiftDetails(this.Schedule, startDateNew);
                await UpdateCalendarMonthYear(month, year);

            }
        }

        private async Task GenerateShiftDetails(ISchedule<IShift> schedule, DateTime startDate)
        {
            int totalHours = await schedule.CalculateTotalWorkHours();

            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

            this.TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";


            if (totalHours > totalHoursByMonth)
            {
                this.HoursMessage = $"You have {totalHours - totalHoursByMonth} hours of overtime, you have to choose which shift to compensate.";
            }
            else
            {
                this.HoursMessage = $"You have {totalHoursByMonth - totalHours} under the total hours for the month.";
            }
        }

        [RelayCommand]
        private async Task CompensateButton()
        {
            if (this.SelectedShift == null)
                return;

            if (this.SelectedShift.ShiftType == ShiftType.RestDay)
            {
                await ShowPopupMessage("Info", "This is a rest day and it can't be compensated!");
                this.SelectedShift = null;
                return;

            }

            await GetDbSchedule();

            if (this.dbSchedule == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, generated a new schedule");
                return;
            }


            DbShift dbShift = await this.shiftRepo.GetSingleShifts( this.dbSchedule.Id,
                                                                    this.SelectedShift.Year,
                                                                    this.SelectedShift.Month,
                                                                    this.SelectedShift.Day);
            if(dbShift == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }

            this.SelectedShift.IsCompensated = !this.SelectedShift.IsCompensated;
            dbShift.IsCompensated = !dbShift.IsCompensated;

            //DbShift dbShift = new DbShift
            //                        {   ShiftType = this.SelectedShift.ShiftType,
            //                            Year = this.SelectedShift.Year,
            //                            Month = this.SelectedShift.Month,
            //                            Day = this.SelectedShift.Day,
            //                            StarTime = this.SelectedShift.StarTime,
            //                            ShiftHour = this.SelectedShift.ShiftHour,
            //                            IsCompensated = this.SelectedShift.IsCompensated,
            //                            IsCurrentMonth = this.SelectedShift.IsCurrentMonth,
            //                            DbScheduleId = this.dbSchedule.Id
            //                        };

            int numberRowsAffected = await this.shiftRepo.UpdateShift(dbShift);

            if (numberRowsAffected == 0)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }
            else
            {
                if (this.SelectedShift.IsCompensated)
                    await ShowPopupMessage("Success", "Your shift has been compensated.");
                else
                    await ShowPopupMessage("Success", "Your shift has been un-compensated.");
            }

            this.SelectedShift = null;
        }

        [RelayCommand]
        private async Task EditShift()
        {
            if (this.SelectedShift == null)
                return;

            var popup = new ShiftEditPopup(this.SelectedShift!);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            if (result is IShift editedShift)
            {
                this.SelectedShift = editedShift;
            }
            else
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }

            await GetDbSchedule();

            if (this.dbSchedule == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }


            DbShift dbShift = await this.shiftRepo.GetSingleShifts(this.dbSchedule.Id,
                                                                    this.SelectedShift.Year,
                                                                    this.SelectedShift.Month,
                                                                    this.SelectedShift.Day);
            if (dbShift == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }

            dbShift.ShiftType = this.SelectedShift.ShiftType;
            dbShift.StarTime = this.SelectedShift.StarTime;
            dbShift.ShiftHour = this.SelectedShift.ShiftHour;


            //DbShift dbShift = new DbShift
            //                        {   ShiftType = this.SelectedShift.ShiftType,
            //                            Year = this.SelectedShift.Year,
            //                            Month = this.SelectedShift.Month,
            //                            Day = this.SelectedShift.Day,
            //                            StarTime = this.SelectedShift.StarTime,
            //                            ShiftHour = this.SelectedShift.ShiftHour,
            //                            IsCompensated = this.SelectedShift.IsCompensated,
            //                            IsCurrentMonth = this.SelectedShift.IsCurrentMonth,
            //                            DbScheduleId = this.dbSchedule.Id
            //                        };

            //Check when saved does it correspond to the current shift that is compensated
            int numberRowsAffected = await this.shiftRepo.UpdateShift(dbShift);

            if(numberRowsAffected == 0)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }

            await ShowPopupMessage("Success", "Your shift has been edit.");
            this.SelectedShift = null;
        }

        [RelayCommand]
        private async Task SaveShiftSchedule()
        {
            int resultNumber = 0;
            await GetDbSchedule();

            if (this.dbSchedule == null)
            {
                await ShowPopupMessage("Error", "Something went wrong, generated new schedule.");
                return;
            }

            DbSchedule existingDbSchedule = await this.scheduleRepo
                                                    .GetScheduleByName(this.dbSchedule.ScheduleName);

            if (existingDbSchedule != null)
            {
                bool overwrite = await Shell.Current
                                            .DisplayAlert(
                                                "Schedule Exists",
                                                $"A schedule for the month {this.dbSchedule.ScheduleName} already exists. Do you want to overwrite it?",
                                                "OK", // Confirm overwrite
                                                "Cancel" // Cancel
                                                );


                if (!overwrite)
                    return;

                resultNumber = await this.scheduleRepo.DeleteSchedule(existingDbSchedule);

                if (resultNumber == 0)
                {
                    await ShowPopupMessage("Error", "Something went wrong, please try again.");
                    return;
                }
            }

            resultNumber = await this.scheduleRepo.AddSchedule(this.dbSchedule);

            if (resultNumber == 0)
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }

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
                    DbScheduleId = this.dbSchedule.Id
                };

                resultNumber+= await this.shiftRepo.AddShift(dbShift);
            }

            if (resultNumber == Schedule.WorkSchedule.Count)
            {
                await ShowPopupMessage("Success", "Your schedule has been saved.");
            }
            else
            {
                await ShowPopupMessage("Error", "Something went wrong, please try again.");
                return;
            }
        }

        //[RelayCommand]
        //private async Task GoBackButton()
        //{
        //    await Shell.Current.GoToAsync("///MainPage");
        //}

        private Task GetDbSchedule()
        {
            int year = this.Schedule.WorkSchedule
                                  .Where(s => s.IsCurrentMonth)
                                  .First()
                                  .Year;

            int month = this.Schedule.WorkSchedule
                                     .Where(s => s.IsCurrentMonth)
                                     .First()
                                     .Month;

            this.dbSchedule = new DbSchedule
                                {
                                    ScheduleName = $"{Provider.GetMonthName(month)} {year}",
                                    Year = year,
                                    Month = month,
                                };

            return Task.CompletedTask;
        }

        private async Task UpdateCalendarMonthYear(int month, int year)
        {
            await Task.Delay(50);   // Just to simulate some delay
            this.CalendarMonthYear = $"Work Schedule for {Provider.GetMonthName(month)} {year} ";
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
