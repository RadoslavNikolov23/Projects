using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.WorkHoursByYears;

namespace WorkChronicle;

public partial class SecondPage : ContentPage
{
    private readonly ISchedule<IShift> schedule; //{ get; set; }

    private ObservableCollection<IShift> SelectedShiftsForRemove { get; set; } = new ObservableCollection<IShift>();
    private ObservableCollection<IShift> CompensatedShifts { get; set; } = new ObservableCollection<IShift>();

    private List<IShift> CompansatedTempList = new List<IShift>();


    public SecondPage(DateTime startDateNew, ISchedule<IShift> scheduleNew, ObservableCollection<IShift> listCompanseteShift)
    {
        InitializeComponent();
       // BindingContext = this.schedule = scheduleNew;
        this.schedule = scheduleNew;
        CompansatedTempList = listCompanseteShift.ToList();

        GenerateShiftDetails(this.schedule, startDateNew);
        BindingContext = this;
    }

    private void GenerateShiftDetails(ISchedule<IShift> schedule, DateTime startDate)
    {
        int totalHours = schedule.TotalWorkHours();

        KeyValuePair<int, string[]> monthByHoursTotal = GetMonthHoursTotal(startDate);
        string monthName = GetMonthName(monthByHoursTotal.Key);
        int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

        ResultsLabel.Text = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

        ShiftCollectionView.ItemsSource = schedule.WorkSchedule;

        if (this.CompansatedTempList.Count == 0)
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
            this.schedule.RemoveShift(shift);
            this.CompansatedTempList.Add(shift);
        }

        ShiftCollectionView.SelectedItems.Clear();
        SelectedShiftsForRemove.Clear();

        DateTime startDate = schedule.WorkSchedule.First().GetDateShift();
        GenerateShiftDetails(schedule, startDate);
    }

    private async void CompensateShiftClicked(object sender, EventArgs e)
    {
        this.CompensatedShifts = new ObservableCollection<IShift>(this.CompansatedTempList.OrderBy(s => s.Year).ThenBy(s => s.Month).ThenBy(s => s.Day).ThenBy(s => s.Hour));
        await Navigation.PushAsync(new ThirdPage(this.CompensatedShifts, this.schedule));
    }

    private string GetMonthName(int month)
    {
        switch (month)
        {
            case 1:
                return "January";
            case 2:
                return "February";
            case 3:
                return "March";
            case 4:
                return "April";
            case 5:
                return "May";
            case 6:
                return "June";
            case 7:
                return "July";
            case 8:
                return "August";
            case 9:
                return "September";
            case 10:
                return "October";
            case 11:
                return "November";
            case 12:
                return "December";
            default:
                return "Unknown";
        }
    }

    private KeyValuePair<int, string[]> GetMonthHoursTotal(DateTime startDate)
    {
        if (startDate.Year == 2024)
        {
            return WorkHoursByYear.Year2024(startDate.Month);
        }
        else if (startDate.Year == 2025)
        {
            return WorkHoursByYear.Year2025(startDate.Month);
        }

        return new KeyValuePair<int, string[]>();
    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
        List<IShift> jsonList = this.schedule.WorkSchedule.ToList();
        string schdeuleJson = JsonSerializer.Serialize(jsonList);
        Preferences.Set("WorkSchedule", schdeuleJson);

        Application.Current.Quit();
    }


}