using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Database;


namespace WorkChronicle;

public partial class CompensateShiftsView : ContentPage
{
    private readonly Schedule schedule;

    private ObservableCollection<IShift> SelectedShiftsToAdd{ get; set; } = new ObservableCollection<IShift>();

    public CompensateShiftsView(Schedule schedule)
    {
        InitializeComponent();
        this.schedule = schedule;
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
    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"ScheduleView");
    }

}