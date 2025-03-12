namespace WorkChronicle.ViewModels
{
    public partial class SchedulePageViewModel:BaseViewModel
    {
        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private IList<object> selectedShiftsForRemove;

        [ObservableProperty]
        private ObservableCollection<IShift> shiftCollectionView;

        [ObservableProperty]
        private string textMessage = "";

        [ObservableProperty]
        private string hoursMessage = "";

        public SchedulePageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
            this.ShiftCollectionView = new ObservableCollection<IShift>();
            this.SelectedShiftsForRemove = new List<object>();
            _ = RefreshThePage();
        }

        public async Task InitializeViewModel()
        {
            await RefreshThePage();
        }

        private async Task RefreshThePage()
        {
            if (Schedule == null)
            {
                Console.WriteLine("Schedule is NULL!");
                return;
            }

            if (Schedule.WorkSchedule == null)
            {
                Console.WriteLine("Schedule.WorkSchedule is NULL!");
                return;
            }

            if (Schedule.WorkSchedule.Count == 0)
            {

                TextMessage = "You have no shifts for this month.";

                //RemoveShiftButton.IsVisible = false; ///TODO
                // CompensateShiftButton.IsVisible = false;   //?TODO
            }
            else
            {
                DateTime startDateNew = Schedule.WorkSchedule.First().GetDateShift();
                await GenerateShiftDetails(this.Schedule, startDateNew);
            }
        }

        private async Task GenerateShiftDetails(ISchedule<IShift> schedule, DateTime startDate)
        {
            int totalHours = await schedule.CalculateTotalWorkHours();

            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

            TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

            this.ShiftCollectionView = new ObservableCollection<IShift>(this.Schedule.WorkSchedule.Where(s => s.IsCompensated == false));

            int compansateShiftsCount = await this.Schedule.TotalCompansatedShifts();

            /* TODO
            if (compansateShiftsCount == 0)
                CompensateShiftButton.IsVisible = false;
            else
                CompensateShiftButton.IsVisible = true;
            */

            if (totalHours > totalHoursByMonth)
            {
                HoursMessage = $"You have {totalHours - totalHoursByMonth} hours of overtime, you have to choose which shift to compensate.";
            }
            else
            {
                HoursMessage = $"You have {totalHoursByMonth - totalHours} under the total hours for the month.";
                //RemoveShiftButton.IsVisible = false; //TODO
            }
        }

        [RelayCommand]
        private async Task RemoveShift()
        {
            if (SelectedShiftsForRemove == null || SelectedShiftsForRemove.Count == 0)
                return;

            await Task.Delay(10);

            foreach (IShift shift in SelectedShiftsForRemove)
            {
                foreach (var s in Schedule.WorkSchedule.Where(s => s.Equals(shift)))
                {
                    s.IsCompensated = true;
                }
            }

            SelectedShiftsForRemove.Clear();

            await RefreshThePage();
        }

        [RelayCommand]
        private async Task CompensateShift()
        {
            await Shell.Current.GoToAsync(nameof(CompensateShiftsPage));
        }

        [RelayCommand]
        private async Task GoBackButton()
        {
            Schedule.WorkSchedule.Clear();
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
}
