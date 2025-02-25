
using System.Diagnostics;

namespace WorkChronicle;

public partial class ScheduleView : ContentPage
{
    private ISchedule<IShift> schedule;

    private ObservableCollection<IShift> SelectedShiftsForRemove { get; set; } = new ObservableCollection<IShift>();

    public ScheduleView(ISchedule<IShift> schedule)
    {

        InitializeComponent();
        this.schedule = schedule;
       // this.SelectedShiftsForRemove ;
        
        RefreshThePage();
    }


    override protected void OnAppearing()
    {
        base.OnAppearing();
        RefreshThePage();
    }

    private void RefreshThePage()
    {
        if (schedule.WorkSchedule.Count == 0)
        {
            TextLabel.Text = "You have no shifts for this month.";
            RemoveShiftButton.IsVisible = false;
            CompensateShiftButton.IsVisible = false;
        }
        else
        {
            DateTime startDateNew = schedule.WorkSchedule.First().GetDateShift();
            GenerateShiftDetails(this.schedule, startDateNew);
        }

        BindingContext = this;

    }

    private void GenerateShiftDetails(ISchedule<IShift> schedule, DateTime startDate)
    {
        int totalHours = schedule.TotalWorkHours();

        KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

        string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
        int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

        TextLabel.Text = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

        ShiftCollectionView.ItemsSource = schedule.WorkSchedule.Where(s => s.isCompensated == false);

        int compansateShiftsCount = this.schedule.TotalCompansatedShifts();

        if (compansateShiftsCount == 0)
            CompensateShiftButton.IsVisible = false;
        else
            CompensateShiftButton.IsVisible = true;


        if (totalHours > totalHoursByMonth)
        {
            HoursLabel.Text = $"You have {totalHours - totalHoursByMonth} hours of overtime, you have to choose which shift to compensate.";
        }
        else
        {
            HoursLabel.Text = $"You have {totalHoursByMonth - totalHours} under the total hours for the month.";
            RemoveShiftButton.IsVisible = false;
        }
    }

    private void OnShiftSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (IShift shift in e.CurrentSelection.Cast<IShift>())
        {
            SelectedShiftsForRemove.Add(shift);
        }

        foreach (var shift in e.PreviousSelection.Cast<IShift>())
        {
            SelectedShiftsForRemove.Remove(shift);
        }
    }

    private void RemoveShiftClicked(object sender, EventArgs e)
    {
        foreach (IShift shift in SelectedShiftsForRemove)
        {
            foreach (var s in this.schedule.WorkSchedule.Where(s => s == shift))
            {
                s.isCompensated = true;
            }
        }

        ShiftCollectionView.SelectedItems.Clear();
        SelectedShiftsForRemove.Clear();

        RefreshThePage();
       // ShiftCollectionView.ItemsSource = schedule.WorkSchedule.Where(s => s.isCompensated == false);
    }

    private async void CompensateShiftClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"CompensateShiftsView");

       // await Navigation.PushAsync(new CompensateShiftsView(schedule));
    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        schedule.WorkSchedule.Clear();
        await Shell.Current.GoToAsync("///MainPage");
    }
}