namespace WorkChronicle.Structure.Configuration
{
    public class ShiftConfiguration(double startDayShift, double startNightShift, double totalShiftHours)
    {
        public double StartDayShift { get; set; } = startDayShift;
        public double StartNightShift { get; set; } = startNightShift;
        public double TotalShiftHours { get; set; } = totalShiftHours;
    }
}
