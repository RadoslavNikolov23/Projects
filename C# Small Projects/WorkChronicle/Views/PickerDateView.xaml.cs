namespace WorkChronicle;

public partial class PickerDateView : ContentPage
{
    private ISchedule<IShift> schedule;

    private readonly IEngine engine;

    public PickerDateView(IEngine engine)
    {
        InitializeComponent();
        this.schedule = new Schedule();
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

        this.schedule= this.engine.CalculateShifts(startDate, cycle);

        await Navigation.PushAsync(new ScheduleView(schedule));
    }
}