namespace WorkChronicle.Controls.Popups
{
    public partial class ShiftInfoPopup : Popup
    {
        public ShiftInfoPopup(IShift shift)
        {
            InitializeComponent();

            ShiftTypeLabel.Text = $"This is a {shift.ShiftType.ToString()}";
            StartTimeLabel.Text = $"You start the shift at {shift.StarTime}.";
            DurationLabel.Text = $"It is {shift.ShiftHour} hours long.";

        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            Close(); // Dismiss the popup
        }
    }
}