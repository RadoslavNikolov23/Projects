namespace WorkChronicle.ViewModels
{
    public partial class CompensateShiftsPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private ObservableCollection<IShift> selectedShiftsToAdd;

        [ObservableProperty]
        private ObservableCollection<IShift> shiftCollectionView;

        public CompensateShiftsPageViewModel(ISchedule<IShift> schedule)
        {
            this.Schedule = schedule;
            this.SelectedShiftsToAdd = new ObservableCollection<IShift>();
            this.ShiftCollectionView = new ObservableCollection<IShift>();
            _ = InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            await RefreshThePage();
        }
        private async Task RefreshThePage()
        {
            await Task.Delay(100);
            this.ShiftCollectionView = (ObservableCollection<IShift>)Schedule.WorkSchedule.Where(s => s.isCompensated == true); ;

            //int compansateShiftsCount = this.schedule.TotalCompansatedShifts();
            //if (compansateShiftsCount == 0)
            //    AddShiftButton.IsVisible = false;
            //else
            //    AddShiftButton.IsVisible = true;


        }

        public async Task HandleSelectionChanged(SelectionChangedEventArgs e)
        {
            await Task.Delay(100);

            foreach (var shift in e.CurrentSelection.Cast<IShift>())
            {
                SelectedShiftsToAdd.Add(shift);
            }

            foreach (var shift in e.PreviousSelection.Cast<IShift>())
            {
                SelectedShiftsToAdd.Remove(shift);
            }
        }

        [RelayCommand]
        private async Task AddShiftButton()
        {
            foreach (IShift shift in SelectedShiftsToAdd)
            {
                foreach (var s in this.Schedule.WorkSchedule.Where(s => s == shift))
                {
                    s.isCompensated = false;
                }
            }

           // ShiftCollectionView.Clear();
            SelectedShiftsToAdd.Clear();

            await RefreshThePage();
        }

        [RelayCommand]
        private async Task GoBackButton()
        {
            // await Navigation.PushAsync(new ScheduleView(schedule));

            await Shell.Current.GoToAsync($"SchedulePage");
        }
    }
}
