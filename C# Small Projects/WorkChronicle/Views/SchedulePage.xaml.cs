namespace WorkChronicle;

public partial class SchedulePage : ContentPage
{

    public SchedulePage(SchedulePageViewModel schedulePageViewModel)
    {

        InitializeComponent();
        BindingContext = schedulePageViewModel;

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is SchedulePageViewModel viewModel)
        {
            _ = viewModel.InitializeViewModel(); // Call a method to refresh data
        }
    }


    private async void ShiftSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var viewModel = (SchedulePageViewModel)BindingContext;

        // Call the ViewModel method to handle the selection change logic
        if (viewModel != null)
        {
            await viewModel.HandleSelectionChanged(e);
        }
    }

}