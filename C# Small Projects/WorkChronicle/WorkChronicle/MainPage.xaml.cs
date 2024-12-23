using WorkChronicle.Logic.Core;
using WorkChronicle.Logic.Core.Contacts;

namespace WorkChronicle
{
    public partial class MainPage : ContentPage
    {
        private IEngine engine;
        private DateTime selectedDate;

        public MainPage()
        {
            InitializeComponent();

            this.engine = new Engine();

            this.selectedDate = DateTime.Now;
            SelectedDateLabel.Text = $"Selected date: {selectedDate:f}";

        }


        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            this.selectedDate = e.NewDate;
            SelectedDateLabel.Text = $"Selected date: {selectedDate:f}";
        }

        private void CalculedWordShifts(object sender, EventArgs e)
        {
            engine.Run(this.selectedDate);
        }
    }

}
