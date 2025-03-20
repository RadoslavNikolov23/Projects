namespace WorkChronicle.ViewModels
{
    using WorkChronicle.Structure.Models.Contracts;
    using WorkChronicle.Structure.Repository.Contracts;

    public partial class CompensateShiftsPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ISchedule<IShift> schedule;

        [ObservableProperty]
        private IList<object> selectedShiftsToAdd;

        [ObservableProperty]
        private ObservableCollection<IShift> shiftCollectionView;

        public CompensateShiftsPageViewModel(ISchedule<IShift> schedule)
        {
            this.schedule = schedule;
            this.SelectedShiftsToAdd = new List<object>();
            this.ShiftCollectionView = new ObservableCollection<IShift>();
            _ = RefreshThePage();
        }

        private async Task RefreshThePage()
        {
            this.ShiftCollectionView = new ObservableCollection<IShift>(this.Schedule.WorkSchedule.Where(s => s.IsCompensated == true));
            await Task.Delay(10);

            //TODO
            //int compansateShiftsCount = this.schedule.TotalCompansatedShifts();
            //if (compansateShiftsCount == 0)
            //    AddShiftButton.IsVisible = false;
            //else
            //    AddShiftButton.IsVisible = true;
        }

       

        [RelayCommand]
        private async Task AddShiftButton()
        {
            foreach (IShift shift in SelectedShiftsToAdd)
            {
                foreach (var s in this.Schedule.WorkSchedule.Where(s => s.Equals(shift)))
                {
                    s.IsCompensated = false;
                }
            }

            SelectedShiftsToAdd.Clear();

            await RefreshThePage();
        }

        [RelayCommand]
        private async Task GoBackButton()
        {
            await Shell.Current.GoToAsync(nameof(SchedulePage));
        }
    }
}
