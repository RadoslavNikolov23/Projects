namespace WorkChronicle;

public partial class SchedulePage : ContentPage
{
    public SchedulePage(SchedulePageViewModel schedulePageViewModel)
    {
        InitializeComponent();
        BindingContext = schedulePageViewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args) //TODO Check if this method is needed?
    {
        base.OnNavigatedTo(args);
        if (BindingContext is SchedulePageViewModel viewModel)
        {
            _ = viewModel.InitializeViewModel();
        }
    }
}