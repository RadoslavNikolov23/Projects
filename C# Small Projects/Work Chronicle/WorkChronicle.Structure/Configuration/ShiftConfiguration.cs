namespace WorkChronicle.Structure.Configuration
{
    public class ShiftConfiguration
    {
        public ShiftConfiguration(double startDayShift, double startNightShift, double totalShiftHours)
        {
            StartDayShift = startDayShift;
            StartNightShift = startNightShift;
            TotalShiftHours = totalShiftHours;
        }

        public double StartDayShift { get; set; }
        public double StartNightShift { get; set; }
        public double TotalShiftHours { get; set; }
    }
}
