namespace WorkChronicle;

public partial class LoadSavedScheduleView : ContentPage
{
    private ISchedule<IShift> schedule;

    public LoadSavedScheduleView()
	{
		InitializeComponent();
        this.schedule = new Schedule ();
    }

    private async void OnLoadSavedButton(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScheduleView(schedule));

    }
}