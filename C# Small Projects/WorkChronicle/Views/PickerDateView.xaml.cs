using System.Collections.ObjectModel;
using System.Text.Json;
using WorkChronicle.Core.Models;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.Database;

namespace WorkChronicle;

public partial class PickerDateView : ContentPage
{
    private readonly Schedule schedule;

    private readonly IEngine engine;

    public ObservableCollection<IShift> ShiftsOC => this.schedule.WorkSchedule;

    public PickerDateView(Schedule schedule, IEngine engine)
    {
        InitializeComponent();
        this.schedule = schedule;
        this.engine = engine;
        BindingContext = this;
    }

    private async void OnCalculateShiftsClicked(object sender, EventArgs e)
    {
        DateTime startDate = StartDatePicker.Date;

        if (WorkSchedulePicker.SelectedIndex == -1)
        {
            ResultsLabel.Text = "Please select a work schedule first.";
            return;
        }

        string selectedSchedule = WorkSchedulePicker.Items[WorkSchedulePicker.SelectedIndex];

        if (string.IsNullOrEmpty(selectedSchedule))
        {
            ResultsLabel.Text = "Please select a work schedule first.";
            return;
        }

        string[] cycle = selectedSchedule.Split('-');
        if (cycle.Length == 0)
        {
            ResultsLabel.Text = "Something went wrong";
            return;
        }

        var temp = this.engine.CalculateShifts(startDate, cycle);

        foreach (var shift in temp.WorkSchedule)
        {
            this.schedule.AddShift(shift);
        }

        await Shell.Current.GoToAsync($"ScheduleView");
    }


}