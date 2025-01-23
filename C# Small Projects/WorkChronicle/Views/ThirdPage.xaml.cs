using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkChronicle.Core.Models.Contracts;


namespace WorkChronicle;

public partial class ThirdPage : ContentPage
{
	public ThirdPage(ObservableCollection<IShift> CompensatedShifts)
	{
		InitializeComponent();

        ShiftCollectionView.ItemsSource = CompensatedShifts;

    }

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}