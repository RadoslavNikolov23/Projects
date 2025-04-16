namespace WorkChronicle.Structure.Configuration
{
    public class ScheduleConfiguration(DateTime startDate, string[] cycle, string firstShift, ShiftConfiguration shiftConfiguration)
    {
        public DateTime StartDate { get; set; } = startDate;
        public string[] Cycle { get; set; } = cycle;
        public string FirstShift { get; set; } = firstShift;
        public ShiftConfiguration ShiftConfiguration { get; set; } = shiftConfiguration;
    }
}
