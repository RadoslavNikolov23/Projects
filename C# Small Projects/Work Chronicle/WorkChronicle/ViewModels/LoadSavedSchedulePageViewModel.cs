namespace WorkChronicle.ViewModels
{
   

    public partial class LoadSavedSchedulePageViewModel:BaseViewModel
    {
        private readonly ScheduleRepositoryDB scheduleRepositoryDB;

        private readonly ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<string> scheduleNames;

        [ObservableProperty]
        private string selectedScheduleName="";



        public LoadSavedSchedulePageViewModel(ISchedule<IShift> schedule,ScheduleRepositoryDB scheduleRepositoryDB)
        {
            this.schedule=schedule;
            this.scheduleRepositoryDB = scheduleRepositoryDB;
            scheduleNames = new ObservableCollection<string>();
        }

        [RelayCommand]
        public async Task LoadScheduleNamesAsync()
        {
            var schedules = await scheduleRepositoryDB.GetSchedulesAsync();
            ScheduleNames.Clear();
            foreach (var s in schedules)
                ScheduleNames.Add(s.ScheduleName);
        }

        [RelayCommand]
        public async Task LoadSaved()
        {
            if (string.IsNullOrEmpty(SelectedScheduleName))
                return;

            var scheduleRecord = await scheduleRepositoryDB.GetScheduleByNameAsync(SelectedScheduleName);

            if (scheduleRecord != null)
            {
                schedule.WorkSchedule.Clear();
                foreach (var shiftRecord in scheduleRecord.ShiftRecords)
                {
                    IShift shift = shiftRecord.ShiftType == ShiftTypes.DayShift
                        ? new DayShift(ShiftType.DayShift, shiftRecord.Year, shiftRecord.Month, shiftRecord.Day, shiftRecord.StarTime, shiftRecord.ShiftHour)
                        : new NightShift(ShiftType.NightShift, shiftRecord.Year, shiftRecord.Month, shiftRecord.Day, shiftRecord.StarTime, shiftRecord.ShiftHour);


                    shift.IsCompensated = shiftRecord.IsCompensated;
                    schedule.WorkSchedule.Add(shift);
                }

                await Shell.Current.GoToAsync(nameof(SchedulePage));
            }
        }
    }
}
