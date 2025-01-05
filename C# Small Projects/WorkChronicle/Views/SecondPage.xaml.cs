using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.WorkHoursByYears;

namespace WorkChronicle;

public partial class SecondPage : ContentPage
{
    private List<string> SelectedShifts { get; set; } = new List<string>();


    public SecondPage(DateTime startDate, string[] cycle)
    {
        InitializeComponent();

        IEngine engine = new Engine();

        ISchedule<IShift> schedule = engine.CalculateShifts(startDate, cycle);

        List<string> shifts = engine.PrintShifts(schedule);
        SelectedShifts = shifts; //TODO
        int totalHours = engine.CalculateTotalHours(schedule);

        KeyValuePair<int, string[]> monthByHoursTotal = new KeyValuePair<int, string[]>();

        if (startDate.Year == 2024)
        {
            monthByHoursTotal = WorkHoursByYear.Year2024(startDate.Month);
        }
        else if (startDate.Year == 2025)
        {
            monthByHoursTotal = WorkHoursByYear.Year2025(startDate.Month);
        }

        string monthName = GetMonthName(monthByHoursTotal.Key);
        int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

        ResultsLabel.Text = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

        ShiftCollectionView.ItemsSource = SelectedShifts;

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
        
    }

    private void RemoveShiftClicked(object sender, EventArgs e)
    {
        


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

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}