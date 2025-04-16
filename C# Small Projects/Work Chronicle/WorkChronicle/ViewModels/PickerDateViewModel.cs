namespace WorkChronicle.ViewModels
{
    public partial class PickerDateViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ISchedule<IShift> schedule;

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

        public ObservableCollection<string> WorkSchedules { get; } = new()
        {
            "Day24Hour",
            "Day-Day",
            "Day-Night",
            "Day-Night-Night"
        };

        public ObservableCollection<string> WorkShift { get; } = new()
        {
            "DayShift",
            "NightShift",
        };

        public PickerDateViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;

            this.selectedStartDate = DateTime.Now;
            this.selectedFirstShift = WorkShift[0];
            this.dayShiftStartTime = new TimeSpan(07, 00, 00);
            this.nightShiftStartTime = new TimeSpan(19, 00, 00);
            this.totalShiftHours = 12;
        }

        [RelayCommand]
        private async Task GenerateSchedule()
        {
            this.Schedule.WorkSchedule.Clear();

            DateTime startDate = SelectedStartDate.Date;

            string[] cycle = await ValidateSchedule();

            if (this.TotalShiftHours <= 0)
            {
                await ShowPopupMessage(AppResources.Error, AppResources.TheTotalShiftHoursMustBepositiveNumber);
                return;
            }

            if ((this.SelectedFirstShift != ShiftType.DayShift.ToString() && this.SelectedFirstShift != ShiftType.NightShift.ToString())
                 || string.IsNullOrEmpty(this.SelectedFirstShift))
            {
                await ShowPopupMessage(AppResources.Error, AppResources.PleaseSelectAValidWorkShift);
                return;
            }

            string dayShift = AppResources.DayShift;
            string nightShift = AppResources.NightShift;

            if (cycle.Length == 1 && this.SelectedFirstShift == ShiftType.NightShift.ToString())
            {
                await ShowPopupMessage(AppResources.Error,
                                String.Format(AppResources.InA24HourdDayScheduleCantBeSelectedAsFirstShift,
                                              nightShift, dayShift));
                return;
            }
            else if ((cycle.Length == 2 && cycle[1] == ShiftType.DayShift.ToString()) && this.SelectedFirstShift == ShiftType.NightShift.ToString())
            {
                await ShowPopupMessage(AppResources.Error,
                                     String.Format(AppResources.InADayDayScheduleCantBeSelectFirstShift,
                                                   nightShift, dayShift));
                return;
            }

            ShiftConfiguration shiftConfiguration = new(this.DayShiftStartTime.Hours,
                                                        this.NightShiftStartTime.Hours,
                                                        this.TotalShiftHours);

            ScheduleConfiguration scheduleConfiguration = new(startDate,
                                                               cycle,
                                                               this.SelectedFirstShift,
                                                               shiftConfiguration);
            try
            {
                IEngine<ISchedule<IShift>> engine = new Engine();
                ISchedule<IShift> tempSchedule = await engine.CalculateShifts(scheduleConfiguration);

                if (tempSchedule == null || tempSchedule.WorkSchedule.Count == 0)
                {
                    await ShowPopupMessage(AppResources.Error, AppResources.AnErrorOccurredWhileCalculatingTheShifts);
                    return;
                }

                foreach (var shift in tempSchedule.WorkSchedule)
                {
                    await this.Schedule.AddShift(shift);
                }

                await Shell.Current.GoToAsync(nameof(SchedulePage));

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GenerateSchedule in the PickerDateViewModel.cs");
                await ShowPopupMessage(AppResources.Error, AppResources.SomethingWentWrongPleaseTryAgain);
            }

        }
        private async Task<string[]> ValidateSchedule()
        {
            if (string.IsNullOrEmpty(this.SelectedSchedule))
            {
                await ShowPopupMessage(AppResources.Error, AppResources.PleaseSelectAScheduleFirst);
                return Array.Empty<string>();
            }

            string[] cycle = SelectedSchedule.Split('-');

            if (cycle.Length == 0)
            {
                await ShowPopupMessage(AppResources.Error, AppResources.PleaseSelectAScheduleFirst);
                return Array.Empty<string>();
            }

            return cycle;
        }

        private async Task ShowPopupMessage(string title, string text)
        {
            await Shell.Current.DisplayAlert(title, text, "OK");
        }
    }
}
