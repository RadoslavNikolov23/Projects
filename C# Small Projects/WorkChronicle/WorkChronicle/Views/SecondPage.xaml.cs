using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Structure.Core;
using WorkChronicle.Structure.Core.Contracts;
using WorkChronicle.Structure.WorkHoursByYears;

namespace WorkChronicle;

public partial class SecondPage : ContentPage
{
	public SecondPage(DateTime startDate, string[] cycle)
	{
		InitializeComponent();

        IEngine engine = new Engine();

        ISchedule<IShift> schedule = engine.CalculateShifts(startDate, cycle);

        List<string> shifts = engine.PrintShifts(schedule);
        int totalHours = engine.CalculateTotalHours(schedule);

        KeyValuePair<int, string[]> monthByHoursTotal = new KeyValuePair<int, string[]>();
        monthByHoursTotal = WorkHoursByYear.Year2024(startDate.Month);
        ResultsLabel.Text = $"Your total hours are: {totalHours}, for the month {monthByHoursTotal.Key} are {monthByHoursTotal.Value[1]}";
        ShiftListView.ItemsSource = shifts;
    }

    

    private async void OnGoBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}