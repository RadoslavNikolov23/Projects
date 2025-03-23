namespace WorkChronicle;

public partial class LoadSavedSchedulePage : ContentPage
{
    public LoadSavedSchedulePage(LoadSavedSchedulePageViewModel loadSavedSchedulePageViewModel)
	{
		InitializeComponent();
        BindingContext=loadSavedSchedulePageViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is LoadSavedSchedulePageViewModel vm)
            await vm.LoadScheduleNamesAsync();
    }
}