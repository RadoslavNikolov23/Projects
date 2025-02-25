namespace WorkChronicle;

public partial class CompensateShiftsView : ContentPage
{
    private readonly ISchedule<IShift> schedule;

    private ObservableCollection<IShift> SelectedShiftsToAdd { get; set; } = new ObservableCollection<IShift>();

    public CompensateShiftsView(ISchedule<IShift> schedule)
    {
        InitializeComponent();
        this.schedule = schedule;
        RefreshThePage();



    }

    override protected void OnAppearing()
    {
        base.OnAppearing();
        RefreshThePage();
    }

    private void RefreshThePage()
    {
        ShiftCollectionView.ItemsSource = schedule.WorkSchedule.Where(s => s.isCompensated == true); ;

        int compansateShiftsCount = this.schedule.TotalCompansatedShifts();
        if (compansateShiftsCount == 0)
            AddShiftButton.IsVisible = false;
        else
            AddShiftButton.IsVisible = true;

        BindingContext = this;
    }

    private void OnShiftSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (IShift shift in e.CurrentSelection.Cast<IShift>())
        {
            SelectedShiftsToAdd.Add(shift);
        }

        foreach (var shift in e.PreviousSelection.Cast<IShift>())
        {
            SelectedShiftsToAdd.Remove(shift);
        }
    }
    private void AddShiftButtonClicked(object sender, EventArgs e)
    {
        foreach (IShift shift in SelectedShiftsToAdd)
        {
            foreach (var s in this.schedule.WorkSchedule.Where(s => s == shift))
            {
                s.isCompensated = false;
            }
        }

        ShiftCollectionView.SelectedItems.Clear();
        SelectedShiftsToAdd.Clear();

        ShiftCollectionView.ItemsSource = schedule.WorkSchedule.Where(s => s.isCompensated == true); ;
    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        // await Navigation.PushAsync(new ScheduleView(schedule));

        await Shell.Current.GoToAsync($"ScheduleView");
    }

}