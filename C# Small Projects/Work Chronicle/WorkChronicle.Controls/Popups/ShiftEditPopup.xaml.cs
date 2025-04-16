namespace WorkChronicle.Controls.Popups
{
    public partial class ShiftEditPopup : Popup, INotifyPropertyChanged
    {
        private readonly IShift shift;
    
        public ShiftEditPopup(IShift shift)
        {
            InitializeComponent();

            this.shift = shift;

            EditShiftLabel.Text = ControlAppResources.EditShift;
            ShiftTypeLabel.Text = ControlAppResources.ShiftType;
            StartTimeLabel.Text = ControlAppResources.StartTime;
            ShiftHourLabel.Text = ControlAppResources.ShiftHour;

            ShiftTypePicker.ItemsSource = Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>().ToList();

            ShiftTypePicker.SelectedItem = shift.ShiftType;
            StartTimePicker.Time = TimeSpan.FromHours(shift.StarTime);
            ShiftDurationPicker.Time = TimeSpan.FromHours(shift.ShiftHour);
        }


        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (ShiftTypePicker.SelectedItem is ShiftType newType)
            {

                if(this.shift.ShiftType == ShiftType.RestDay && newType == ShiftType.RestDay)
                {
                    WarningLabel.Text = ControlAppResources.CantEditRestDays;
                    WarningLabel.IsVisible = true;
                    return;
                }
                else if(this.shift.ShiftType == ShiftType.RestDay 
                         && ( newType==ShiftType.DayShift || newType == ShiftType.NightShift))
                {
                    shift.ShiftType = newType;
                    shift.StarTime = StartTimePicker.Time.TotalHours;
                    shift.ShiftHour = ShiftDurationPicker.Time.TotalHours;
                    shift.IsCompensated = false;

                    Close(this.shift);
                }
                else if((this.shift.ShiftType == ShiftType.DayShift 
                            || this.shift.ShiftType == ShiftType.NightShift) 
                             && newType == ShiftType.RestDay)
                {
                    shift.ShiftType = newType;
                    shift.StarTime = 0;
                    shift.ShiftHour = 0;
                    shift.IsCompensated = false;

                    Close(this.shift);
                }
                else if((this.shift.ShiftType == ShiftType.DayShift
                        || this.shift.ShiftType == ShiftType.NightShift) 
                        && (newType == ShiftType.DayShift
                        || newType == ShiftType.NightShift))
                {
                    shift.ShiftType = newType;
                    shift.StarTime = StartTimePicker.Time.TotalHours;
                    shift.ShiftHour = ShiftDurationPicker.Time.TotalHours;
                    shift.IsCompensated = false;

                    Close(this.shift);
                }
                else
                {
                    WarningLabel.Text = ControlAppResources.SomethingWentWrongTryAgain;
                    WarningLabel.IsVisible = true;
                }
                return;
            }
            else
            {
                WarningLabel.Text = ControlAppResources.PleaseSelectValidShiftType;
                WarningLabel.IsVisible = true;
                return;
            }
        }
    }
}