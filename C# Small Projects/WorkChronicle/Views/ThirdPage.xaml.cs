using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;


namespace WorkChronicle;

public partial class ThirdPage : ContentPage
{
    private readonly ISchedule<IShift> _scheduleCom;// { get; set; }

    private ObservableCollection<IShift> SelectedShiftsToAdd{ get; set; } = new ObservableCollection<IShift>();
    private ObservableCollection<IShift> CompensatedShiftsInThirdPage { get; set; } = new ObservableCollection<IShift>();

    public ThirdPage(ObservableCollection<IShift> CompensatedShifts, ISchedule<IShift> schedule)
	{
		InitializeComponent();

        BindingContext = _scheduleCom = schedule;

        ShiftCollectionView.ItemsSource = CompensatedShifts;
        CompensatedShiftsInThirdPage=CompensatedShifts;

        if (CompensatedShifts.Count == 0)
            AddShiftButton.IsVisible = false;
        else
            AddShiftButton.IsVisible = true;

    }

    private void OnShiftSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (IShift shift in e.CurrentSelection.Cast<IShift>())
        {
            // if (!SelectedShiftsForRemove.Contains(shift))
            // {
            SelectedShiftsToAdd.Add(shift);
            // }
        }

        foreach (var shift in e.PreviousSelection.Cast<IShift>())
        {
            SelectedShiftsToAdd.Remove(shift);
        }

        //ShiftCollectionView.SelectedItems.Clear();

    }

    private void AddShiftButtonClicked(object sender, EventArgs e)
    {
        foreach (IShift shift in SelectedShiftsToAdd)
        {
            this._scheduleCom.AddShift(shift);
            this.CompensatedShiftsInThirdPage.Remove(shift);
        }

        ShiftCollectionView.SelectedItems.Clear();
        SelectedShiftsToAdd.Clear();


    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        DateTime startDate = this._scheduleCom.WorkSchedule.First().GetDateShift();

        await Navigation.PushAsync(new SecondPage(startDate, _scheduleCom));
    }

}