namespace WorkChronicle;

public partial class LoadSavedScheduleView : ContentPage
{
    private readonly ISchedule<IShift> schedule;

    public LoadSavedScheduleView(ISchedule<IShift> schedule)
	{
		InitializeComponent();
        this.schedule = schedule;
    }

    private async void OnLoadSavedButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ScheduleView");

       // await Navigation.PushAsync(new ScheduleView(schedule));

    }
}