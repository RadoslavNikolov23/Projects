namespace WorkChronicle;

public partial class PickerDatePage : ContentPage
{

    public PickerDatePage(PickerDatePageViewModel pickerDatePageViewModel)
    {
        InitializeComponent();
        BindingContext = pickerDatePageViewModel;
    }

    
}