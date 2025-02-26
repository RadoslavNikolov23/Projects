using WorkChronicle.ViewModels;

namespace WorkChronicle;

public partial class CompensateShiftsPage : ContentPage
{
    public CompensateShiftsPage(CompensateShiftsPageViewModel compensateShiftsPageViewModel)
    {
        InitializeComponent();
        BindingContext= compensateShiftsPageViewModel;
    }
    private async void ShiftSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var viewModel = (CompensateShiftsPageViewModel)BindingContext;
        if (viewModel != null)
        {
            await viewModel.HandleSelectionChanged(e);
        }
    }
}