namespace WorkChronicle;

public partial class CompensateShiftsPage : ContentPage
{
    public CompensateShiftsPage(CompensateShiftsPageViewModel compensateShiftsPageViewModel)
    {
        InitializeComponent();
        BindingContext = compensateShiftsPageViewModel;
    }

}
