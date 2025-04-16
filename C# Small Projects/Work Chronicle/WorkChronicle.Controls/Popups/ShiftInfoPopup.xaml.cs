namespace WorkChronicle.Controls.Popups
{
    public partial class ShiftInfoPopup : Popup
    {
        public ShiftInfoPopup(IShift shift)
        {
            InitializeComponent();

            InformationLabel.Text = ControlAppResources.Information;

            string shiftTypeString = shift.ShiftType switch
            {
                ShiftType.DayShift => ControlAppResources.DayShift,
                ShiftType.NightShift => ControlAppResources.NightShift,
                _ => ControlAppResources.RestDay,
            };

            ShiftTypeLabel.Text = String.Format(ControlAppResources.ThisIsA, shiftTypeString);

            if (shift.ShiftType == ShiftType.DayShift
               || shift.ShiftType == ShiftType.NightShift)
            {
                if(shift.IsCompensated)
                {
                    StartTimeLabel.Text = ControlAppResources.ThisShiftIsCompensated;
                    DurationLabel.Text = String.Format(ControlAppResources.YouHaveCompensatedHours, shift.ShiftHour);
                }
                else
                {
                    string starTimeString = FormatTimeOfDay(shift.StarTime);

                    StartTimeLabel.Text = String.Format(ControlAppResources.YouStartTheShiftAt, starTimeString);
                    DurationLabel.Text = String.Format(ControlAppResources.ItIsHoursLong, shift.ShiftHour);
                }
            }
            else
            {
                StartTimeLabel.Text = "";
                DurationLabel.Text = "";
            }
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }
        public static string FormatTimeOfDay(double hourValue)
        {
            TimeSpan time = TimeSpan.FromHours(hourValue);
            DateTime timeOfDay = DateTime.Today.Add(time);

            var culture = Thread.CurrentThread.CurrentUICulture;
            return timeOfDay.ToString("t", culture); // Short time pattern
        }
    }
}