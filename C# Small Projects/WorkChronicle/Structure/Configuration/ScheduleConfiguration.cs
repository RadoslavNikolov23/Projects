namespace WorkChronicle.Structure.Configuration
{

    public class ScheduleConfiguration
    {
        public ScheduleConfiguration(DateTime startDate, string[] cycle, string firstShift, ShiftConfiguration shiftConfiguration)
        {
            StartDate = startDate;
            Cycle = cycle;
            FirstShift = firstShift;
            ShiftConfiguration = shiftConfiguration;
        }

        public DateTime StartDate { get; set; }
        public string[] Cycle { get; set; }
        public string FirstShift { get; set; }
        public ShiftConfiguration ShiftConfiguration { get; set; }
    }
}
