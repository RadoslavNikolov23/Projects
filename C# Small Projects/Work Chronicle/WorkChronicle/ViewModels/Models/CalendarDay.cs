namespace WorkChronicle.ViewModels.Models
{
    public partial class CalendarDay : ObservableObject
    {
        [ObservableProperty]
        public string day;

        [ObservableProperty]
        public string backgroundColor;

        [ObservableProperty]
        public bool isCurrentMonth;
    }
}
