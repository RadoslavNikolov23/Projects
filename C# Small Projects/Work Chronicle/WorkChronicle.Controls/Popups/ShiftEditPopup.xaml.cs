namespace WorkChronicle.Controls.Popups
{
    public partial class ShiftEditPopup : Popup
    {
        private readonly IShift shift;

        public ShiftEditPopup(IShift shift)
        {
            InitializeComponent();

            this.shift = shift;

            // Populate the ShiftTypePicker
            ShiftTypePicker.ItemsSource = Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>().ToList();

            //if (shift.ShiftType == ShiftType.RestDay)
            //{
            //    WarningLabel.Text = "Rest day can the edit for work.";
            //    WarningLabel.IsVisible = true;
            //}

            // Pre-fill values
            ShiftTypePicker.SelectedItem = shift.ShiftType;
            StartTimePicker.Time = TimeSpan.FromHours(shift.StarTime);
            ShiftDurationPicker.Time = TimeSpan.FromHours(shift.ShiftHour);

            // - TO EXAMPLE ShiftHourEntry.Time = shift.ShiftHour.ToString("F1");
            // CompensateCheckBox.IsChecked = shift.IsCompensated;
        }


        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(); // Don't save anything
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (ShiftTypePicker.SelectedItem is ShiftType newType)
            {
                if(this.shift.ShiftType == ShiftType.RestDay && newType == ShiftType.RestDay)
                {
                    WarningLabel.Text = "Can't edit rest days. Try again!";
                    WarningLabel.IsVisible = true;
                    return;
                }

                if(this.shift.ShiftType == ShiftType.RestDay && newType != ShiftType.RestDay)
                {
                    shift.ShiftType = newType;
                    shift.StarTime = StartTimePicker.Time.TotalHours;
                    shift.ShiftHour = ShiftDurationPicker.Time.TotalHours;

                    Close(this.shift);
                }

                if((this.shift.ShiftType == ShiftType.DayShift 
                    || this.shift.ShiftType == ShiftType.NightShift) && newType == ShiftType.RestDay)
                {
                    shift.ShiftType = newType;
                    shift.StarTime = 0;
                    shift.ShiftHour = 0;

                    Close(this.shift);
                }

                if(this.shift.ShiftType == ShiftType.RestDay 
                    && ( newType==ShiftType.DayShift || newType == ShiftType.NightShift))
                {
                    shift.ShiftType = newType;
                    shift.StarTime = StartTimePicker.Time.TotalHours;
                    shift.ShiftHour = ShiftDurationPicker.Time.TotalHours;

                    Close(this.shift);
                }

                WarningLabel.Text = "Something went wrong. Try again!";
                WarningLabel.IsVisible = true;
                return;

                //if (newType != shift.ShiftType
                //    || StartTimePicker.Time.TotalHours != shift.StarTime
                //    || ShiftDurationPicker.Time.TotalHours != shift.ShiftHour)
                //{
                //    shift.ShiftType = newType;
                //    shift.StarTime = StartTimePicker.Time.TotalHours;
                //    shift.ShiftHour = ShiftDurationPicker.Time.TotalHours;

                //    // shift.ShiftHour = newHours;

                //    // shift.IsCompensated = CompensateCheckBox.IsChecked;

                //    // Update background color logic - TO Check
                //                        //shift.BackgroundColor = !shift.IsCurrentMonth ? Colors.LightGray
                //                        //                       : shift.IsCompensated ? Colors.LightBlue
                //                        //                       : shift.ShiftType == ShiftType.DayShift ? Colors.LightGreen
                //                        //                       : shift.ShiftType == ShiftType.NightShift ? Colors.YellowGreen
                //                        //                       : Colors.White;

                //    Close(this.shift);
                //}
                //else
                //{
                //    WarningLabel.Text = "Invalid input. Please check the values.";
                //    WarningLabel.IsVisible = true;
                //}
            }
            else
            {
                WarningLabel.Text = "Please select a valid shift type.";
                WarningLabel.IsVisible = true;
                return;
            }
        }

    }
}