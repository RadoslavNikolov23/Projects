namespace WorkChronicle.Controls
{
    public partial class ShiftCompensationPopup : Popup
    {
        private readonly IShift shift;

        public ShiftCompensationPopup(IShift shift)
        {
            InitializeComponent();
            this.shift = shift;

            // Populate the ShiftTypePicker
            ShiftTypePicker.ItemsSource = Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>().ToList();

            if (shift.ShiftType == ShiftType.RestDay)
            {
                WarningLabel.Text = "Editing rest days is limited.";
                WarningLabel.IsVisible = true;
            }

            // Pre-fill values
            ShiftTypePicker.SelectedItem = shift.ShiftType;
            StartTimePicker.Time = TimeSpan.FromHours(shift.StarTime);
            ShiftHourEntry.Text = shift.ShiftHour.ToString("F1");
            CompensateCheckBox.IsChecked = shift.IsCompensated;
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(); // Don't save anything
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (ShiftTypePicker.SelectedItem is ShiftType newType &&
                double.TryParse(ShiftHourEntry.Text, out double newHours))
            {
                shift.ShiftType = newType;
                shift.StarTime = StartTimePicker.Time.TotalHours;
                shift.ShiftHour = newHours;
                shift.IsCompensated = CompensateCheckBox.IsChecked;

                // Update background color logic
                shift.BackgroundColor = !shift.IsCurrentMonth ? Colors.LightGray
                                       : shift.IsCompensated ? Colors.LightBlue
                                       : shift.ShiftType == ShiftType.DayShift ? Colors.LightGreen
                                       : shift.ShiftType == ShiftType.NightShift ? Colors.YellowGreen
                                       : Colors.White;

                Close();
            }
            else
            {
                WarningLabel.Text = "Invalid input. Please check the values.";
                WarningLabel.IsVisible = true;
            }
        }
    }
}