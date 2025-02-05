using CommunityToolkit.Mvvm.Input;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using WorkChronicle.Core.Models;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.Database;
using WorkChronicle.Structure.Models;
using WorkChronicle.Structure.WorkHoursByYears;

namespace WorkChronicle;

public partial class ScheduleView : ContentPage
{
    private readonly Schedule schedule;

    private ObservableCollection<IShift> SelectedShiftsForRemove { get; set; } = new ObservableCollection<IShift>();

    public ScheduleView(Schedule schedule)
    {
        InitializeComponent();
       this.schedule = schedule;

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

        DateTime startDate = schedule.WorkSchedule.First().GetDateShift();
        GenerateShiftDetails(schedule, startDate);
    }

    private async void CompensateShiftClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"CompensateShiftsView");
    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }



}