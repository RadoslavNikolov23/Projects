namespace WorkChronicle;

public partial class LoadSavedSchedulePage : ContentPage
{
    public LoadSavedSchedulePage(LoadSavedSchedulePageViewModel loadSavedSchedulePageViewModel)
	{
		InitializeComponent();
        BindingContext=loadSavedSchedulePageViewModel;
    }

}