namespace WorkChronicle;

public partial class SchedulePage : ContentPage
{

    public SchedulePage(SchedulePageViewModel schedulePageViewModel)
    {

        InitializeComponent();
        BindingContext = schedulePageViewModel;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // If you are using a ViewModel constructor that needs data, you can reset it here.
        // Ensure you are re-binding the context if necessary.
       // var viewModel = (SchedulePageViewModel)BindingContext;
        //viewModel?.RefreshThePage(); // Call a method to refresh the data/UI
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