namespace WorkChronicle.ViewModels
{
    public partial class AboutViewModel:BaseViewModel
    {
        [ObservableProperty]
        private string aboutText;

        public AboutViewModel()
        {
            this.AboutText = @" 
        This application is designed to help you manage and track your work shifts with ease. It supports automatic schedule generation for several shift patterns, including:
            - 24-Hour Day Shift.
            - Day–Day Shift.
            - Day–Night Shift.
            - Day–Night–Night Shift.
        Simply enter your shift pattern, start time, and shift duration. Based on that, the app generates a complete monthly schedule.
        If no schedule is available on startup, a blank calendar is shown. You can save and load schedules for different months. 
        In the Edit Page, you can:
            - Modify existing shifts
            - Compensate shifts
            - Change a shift's type (Day, Night, or Rest Day)
        The app also calculates your monthly working hours and compares them with required hours in Bulgaria. 
        You’ll be notified if you're under or above the expected total.
        Legend:  
            🔲 Light Gray – Day from previous/next month  
            ⬜ White – Rest Day  
            🟩 Light Green – Day Shift  
            🟥 Light Coral – Night Shift  
            🟦 Light Blue – Compensated Shift";
        }

    }
}
