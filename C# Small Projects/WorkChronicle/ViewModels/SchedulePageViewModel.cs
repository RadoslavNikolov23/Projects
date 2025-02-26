namespace WorkChronicle.ViewModels
{
    public partial class SchedulePageViewModel:BaseViewModel
    {
        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<IShift> selectedShiftsForRemove;

        [ObservableProperty]
        private ObservableCollection<IShift> shiftCollectionView;

        [ObservableProperty]
        private string textMessage = "";

        [ObservableProperty]
        private string hoursMessage = "";

        public SchedulePageViewModel()
        {
            
        }
        public SchedulePageViewModel(ISchedule<IShift> schedule):base()
        {
            this.schedule = schedule;
            this.ShiftCollectionView = new ObservableCollection<IShift>();
            this.SelectedShiftsForRemove = new ObservableCollection<IShift>();
            _ = InitializeViewModel(); 

        }

        private async Task InitializeViewModel()
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
            //await Task.Delay(100);

            int totalHours = await schedule.TotalWorkHours();

            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

            TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

            this.ShiftCollectionView = new ObservableCollection<IShift>(this.Schedule.WorkSchedule.Where(s => s.isCompensated == false));

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


       // public AsyncRelayCommand RemoveShiftCommand => new AsyncRelayCommand(RemoveShift);

        [RelayCommand]
        private async Task RemoveShift()
        {
            await Task.Delay(100);
            foreach (IShift shift in SelectedShiftsForRemove)
            {
                foreach (var s in Schedule.WorkSchedule.Where(s => s == shift))
                {
                    s.isCompensated = true;  
                }
            }
            SelectedShiftsForRemove.Clear();

            await RefreshThePage();
        }

        public async Task HandleSelectionChanged(SelectionChangedEventArgs e)
        {
            await Task.Delay(100);

            foreach (var shift in e.CurrentSelection.Cast<IShift>())
            {
                if (!SelectedShiftsForRemove.Contains(shift))
                    SelectedShiftsForRemove.Add(shift);  
            }

            foreach (var shift in e.PreviousSelection.Cast<IShift>())
            {
                SelectedShiftsForRemove.Remove(shift); 
            }
        }

        

        //[RelayCommand]
        //private async Task RemoveShiftClicked(object sender, EventArgs e)
        //{
        //    foreach (IShift shift in SelectedShiftsForRemove)
        //    {
        //        foreach (var s in this.schedule.WorkSchedule.Where(s => s == shift))
        //        {
        //            s.isCompensated = true;
        //        }
        //    }

        //    ShiftCollectionView.SelectedItems.Clear();
        //    SelectedShiftsForRemove.Clear();

        //    RefreshThePage();
        //    // ShiftCollectionView.ItemsSource = schedule.WorkSchedule.Where(s => s.isCompensated == false);
        //}

        [RelayCommand]
        private async Task CompensateShift()
        {
            await Shell.Current.GoToAsync($"CompensateShiftsPage");
        }


        [RelayCommand]
        private async Task GoBackButton()
        {
            Schedule.WorkSchedule.Clear();
            await Shell.Current.GoToAsync("///MainPage");
        }

    }
}
